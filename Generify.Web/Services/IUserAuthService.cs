using Generify.Models.Management;
using System.Threading.Tasks;

namespace Generify.Web.Services
{
    public interface IUserAuthService
    {
        Task<string> TryLoginAsync(string userName, string password);
        Task LoginAsync(User user);

        Task LogoutAsync();
    }
}