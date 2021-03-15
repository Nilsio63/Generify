using Generify.Models.Management;
using System.Threading.Tasks;

namespace Generify.Services
{
    public interface IUserAuthService
    {
        Task<bool> IsUserLoggedInAsync();
        Task<User> GetCurrentUserAsync();

        Task<string> TryLoginAsync(string userName, string password);
        Task LoginAsync(User user);

        Task LogoutAsync();
    }
}
