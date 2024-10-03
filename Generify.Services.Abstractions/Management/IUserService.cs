using Generify.Models.Management;
using System.Threading.Tasks;

namespace Generify.Services.Abstractions.Management;

public interface IUserService
{
    Task<User> GetByIdAsync(string userId);

    Task<User> SaveAsync(User user);
}
