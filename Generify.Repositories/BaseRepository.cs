using Generify.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Repositories
{
    public abstract class BaseRepository<T>
        where T : Entity
    {
        protected IQueryable<T> BaseSelect => DataContext.Set<T>();
        protected GenerifyDataContext DataContext { get; }

        public BaseRepository(GenerifyDataContext dataContext)
        {
            DataContext = dataContext;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await BaseSelect
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
