using Generify.Models.Management;
using System.Threading.Tasks;

namespace Generify.Services.Interfaces.Management
{
    public interface IUserService
    {
        Task<User> GetByUserNameAsync(string userName);

        Task<UserCreationResult> TryCreateUser(string userName, string password);
        Task<User> SaveAsync(User user);
    }
}
