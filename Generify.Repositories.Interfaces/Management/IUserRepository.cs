using Generify.Models.Management;
using System.Threading.Tasks;

namespace Generify.Repositories.Interfaces.Management
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
        Task SaveAsync(User user);
    }
}
