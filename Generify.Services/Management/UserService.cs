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
