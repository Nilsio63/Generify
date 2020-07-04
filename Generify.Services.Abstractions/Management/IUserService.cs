using Generify.Models.Management;
using System.Threading.Tasks;

namespace Generify.Services.Abstractions.Management
{
    public interface IUserService
    {
        Task<User> GetByIdAsync(string userName);
        Task<User> GetByLoginAsync(string userName, string password);

        Task<UserCreationResult> TryCreateUser(string userName, string password);
        Task<User> SaveAsync(User user);
    }
}
