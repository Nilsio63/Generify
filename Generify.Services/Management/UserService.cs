using Generify.Models.Management;
using Generify.Repositories.Abstractions.Management;
using Generify.Services.Abstractions.Management;
using System.Threading.Tasks;

namespace Generify.Services.Management;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<User?> GetByIdAsync(string userId)
    {
        return await _userRepo.GetByIdAsync(userId);
    }

    public async Task<User> SaveAsync(User user)
    {
        return await _userRepo.SaveAsync(user);
    }
}
