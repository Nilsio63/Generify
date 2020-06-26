using Generify.Models.Management;
using Generify.Repositories.Interfaces.Management;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
            userName = userName.ToLower();

            return await BaseSelect
                .Where(o => o.UserNameInternal == userName)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserNameTakenAsync(string userName)
        {
            return await GetByUserNameAsync(userName) != null;
        }

        protected override void BeforeSave(IEnumerable<User> objectList)
        {
            base.BeforeSave(objectList);

            foreach (User user in objectList)
            {
                user.UserNameInternal = user.UserNameDisplay.ToLower();
            }
        }
    }
}
