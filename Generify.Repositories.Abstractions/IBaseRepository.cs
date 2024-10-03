using Generify.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generify.Repositories.Abstractions;

public interface IBaseRepository<T>
    where T : Entity
{
    Task<List<T>> GetAllAsync();
    Task<List<T>> GetAllByIdListAsync(IEnumerable<string> idList);
    Task<T> GetByIdAsync(string id);

    Task<List<T>> SaveRangeAsync(IEnumerable<T> objectList);
    Task<T> SaveAsync(T obj);
    Task DeleteRangeAsync(IEnumerable<T> objectList);
    Task DeleteRangeAsync(IEnumerable<string> idList);
    Task DeleteAsync(T obj);
    Task DeleteAsync(string id);
}
