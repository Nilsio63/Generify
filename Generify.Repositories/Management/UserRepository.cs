using Generify.Models.Management;
using Generify.Repositories.Interfaces.Management;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Repositories.Management
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(GenerifyDataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await BaseSelect
                .Where(o => o.UserName == userName)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserNameTakenAsync(string userName)
        {
            return await GetByUserNameAsync(userName) != null;
        }
    }
}
