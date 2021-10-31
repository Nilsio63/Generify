using Generify.Models.Management;
using System.Threading.Tasks;

namespace Generify.Services.Abstractions.Management
{
    public interface IUserContextAccessor
    {
        Task<User> GetCurrentUserAsync();
    }
}
