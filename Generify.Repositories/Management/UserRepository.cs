using Generify.Models.Management;
using Generify.Repositories.Interfaces;
using Generify.Repositories.Interfaces.Management;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Repositories.Management
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        protected override string FolderName => "users";

        public UserRepository(IFileAccess fileAccess)
            : base(fileAccess)
        {
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await ReadEntitiesAsync()
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task SaveAsync(User user)
        {
            await WriteEntityAsync(user, o => o.Id == user.Id);
        }
    }
}
