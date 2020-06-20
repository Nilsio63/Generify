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

        public async Task<User> GetByIdAsync(string id)
        {
            return await BaseSelect
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task SaveAsync(User user)
        {
            DataContext.Users.Add(user);

            await DataContext.SaveChangesAsync();
        }
    }
}
