using Generify.Models.Management;
using Generify.Repositories.Abstractions.Management;
using Generify.Services.Abstractions.Management;

namespace Generify.Services.Management;

public class UserService(IUserRepository userRepo) : IUserService
{
    public async Task<User?> GetByIdAsync(string userId)
    {
        return await userRepo.GetByIdAsync(userId);
    }

    public async Task<User> SaveAsync(User user)
    {
        return await userRepo.SaveAsync(user);
    }
}
