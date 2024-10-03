using Generify.Models;
using Generify.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T>
    where T : Entity
{
    protected virtual IQueryable<T> BaseSelect => DataContext.Set<T>();
    protected DbSet<T> DbSet => DataContext.Set<T>();
    protected GenerifyDataContext DataContext { get; }

    public BaseRepository(GenerifyDataContext dataContext)
    {
        DataContext = dataContext;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await BaseSelect.ToListAsync();
    }

    public async Task<List<T>> GetAllByIdListAsync(IEnumerable<string> idList)
    {
        return await BaseSelect
            .Where(o => idList.Contains(o.Id))
            .ToListAsync();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        return await BaseSelect
            .Where(o => o.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<T> SaveAsync(T obj)
    {
        await SaveRangeAsync(new T[] { obj });

        return obj;
    }

    public async Task<List<T>> SaveRangeAsync(IEnumerable<T> objectList)
    {
        objectList = objectList.ToArray();

        BeforeSave(objectList);

        foreach (T obj in objectList)
        {
            if (string.IsNullOrWhiteSpace(obj.Id))
            {
                DbSet.Add(obj);
            }
            else
            {
                DbSet.Update(obj);
            }
        }

        await DataContext.SaveChangesAsync();

        return objectList.ToList();
    }

    protected virtual void BeforeSave(IEnumerable<T> objectList)
    {
    }

    public async Task DeleteRangeAsync(IEnumerable<T> objectList)
    {
        DbSet.RemoveRange(objectList);

        await DataContext.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<string> idList)
    {
        var objectList = await GetAllByIdListAsync(idList);

        await DeleteRangeAsync(objectList);
    }

    public async Task DeleteAsync(T obj)
    {
        await DeleteRangeAsync(new T[] { obj });
    }

    public async Task DeleteAsync(string id)
    {
        await DeleteRangeAsync(new string[] { id });
    }
}
