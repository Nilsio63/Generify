using Generify.Models.Management;

namespace Generify.Services.Abstractions.Management;

public interface IUserContextAccessor
{
    Task<User?> GetCurrentUserAsync();
}
