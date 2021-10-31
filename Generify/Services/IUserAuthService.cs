using Generify.Models.Management;
using Generify.Services.Abstractions.Management;
using System.Threading.Tasks;

namespace Generify.Services
{
    public interface IUserAuthService : IUserContextAccessor
    {
        Task<bool> IsUserLoggedInAsync();

        Task<string> TryLoginAsync(string userName, string password);
        Task LoginAsync(User user);

        Task LogoutAsync();
    }
}
