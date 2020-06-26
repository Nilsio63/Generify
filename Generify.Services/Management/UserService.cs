using Generify.Models.Management;
using Generify.Repositories.Interfaces.Management;
using Generify.Services.Interfaces.Management;
using Generify.Services.Interfaces.Security;
using System.Threading.Tasks;

namespace Generify.Services.Management
{
    public class UserService : IUserService
    {
        private readonly IHashEncoder _hashEncoder;
        private readonly IUserRepository _userRepo;

        public UserService(IHashEncoder hashEncoder,
            IUserRepository userRepo)
        {
            _hashEncoder = hashEncoder;
            _userRepo = userRepo;
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _userRepo.GetByUserNameAsync(userName);
        }

        public async Task<bool> IsUserNameTakenAsync(string userName)
        {
            return await _userRepo.IsUserNameTakenAsync(userName);
        }

        public async Task<UserCreationResult> TryCreateUser(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return new UserCreationResult("The user name is required");
            }

            if (await _userRepo.IsUserNameTakenAsync(userName))
            {
                return new UserCreationResult($"The user name {userName} is already taken");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return new UserCreationResult("The password is required");
            }

            if (password.Length < 6)
            {
                return new UserCreationResult("The password must contain at least 6 characters");
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
