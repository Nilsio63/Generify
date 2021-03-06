﻿using Generify.Models.Management;
using Generify.Repositories.Abstractions.Management;
using Generify.Services.Abstractions.Management;
using Generify.Services.Abstractions.Security;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Services.Management
{
    public class UserService : IUserService
    {
        private readonly IHashEncoder _hashEncoder;
        private readonly IPasswordValidator _passwordValidator;
        private readonly IUserRepository _userRepo;

        public UserService(IHashEncoder hashEncoder,
            IPasswordValidator passwordValidator,
            IUserRepository userRepo)
        {
            _hashEncoder = hashEncoder;
            _passwordValidator = passwordValidator;
            _userRepo = userRepo;
        }

        public async Task<User> GetByIdAsync(string userId)
        {
            return await _userRepo.GetByIdAsync(userId);
        }

        public async Task<User> GetByLoginAsync(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            User user = await _userRepo.GetByUserNameAsync(userName);

            if (user == null)
            {
                return null;
            }

            byte[] passwordHash = _hashEncoder.EncodeToHash(password);

            if (user.PasswordHash.SequenceEqual(passwordHash))
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IsUserNameTakenAsync(string userName)
        {
            return await _userRepo.IsUserNameTakenAsync(userName);
        }

        public async Task<UserCreationResult> TryCreateUser(string userName, string password)
        {
            userName = userName?.Trim();

            if (string.IsNullOrWhiteSpace(userName))
            {
                return new UserCreationResult("The user name is required");
            }

            if (await _userRepo.IsUserNameTakenAsync(userName))
            {
                return new UserCreationResult($"The user name {userName} is already taken");
            }

            string pwError = _passwordValidator.ValidatePassword(password);

            if (!string.IsNullOrWhiteSpace(pwError))
            {
                return new UserCreationResult(pwError);
            }

            byte[] passwordHash = _hashEncoder.EncodeToHash(password);

            var user = new User
            {
                UserNameDisplay = userName,
                PasswordHash = passwordHash
            };

            await _userRepo.SaveAsync(user);

            return new UserCreationResult(user);
        }

        public async Task<User> SaveAsync(User user)
        {
            return await _userRepo.SaveAsync(user);
        }
    }
}
