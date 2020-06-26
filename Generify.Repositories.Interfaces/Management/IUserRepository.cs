using Generify.Models.Management;
using System.Threading.Tasks;

namespace Generify.Repositories.Interfaces.Management
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByUserNameAsync(string userName);
        Task<bool> IsUserNameTakenAsync(string userName);
    }
}
