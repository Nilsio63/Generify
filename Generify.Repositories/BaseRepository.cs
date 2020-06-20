using System.Linq;

namespace Generify.Repositories
{
    public abstract class BaseRepository<T>
        where T : class
    {
        protected IQueryable<T> BaseSelect => DataContext.Set<T>();
        protected GenerifyDataContext DataContext { get; }

        public BaseRepository(GenerifyDataContext dataContext)
        {
            DataContext = dataContext;
        }
    }
}
