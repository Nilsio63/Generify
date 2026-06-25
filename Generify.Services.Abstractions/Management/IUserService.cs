using Generify.Models.Management;

namespace Generify.Services.Abstractions.Management;

public interface IUserService
{
    Task<User?> GetByIdAsync(string userId);
    Task<User> SaveAsync(User user);
}
