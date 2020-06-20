using Generify.Repositories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generify.Repositories.Interfaces
{
    public interface IFileAccess
    {
        string GetBasePath();

        IAsyncEnumerable<FileEntry<T>> ReadJsonFilesAsync<T>(string directoryPath);
        Task WriteJsonAsync(string filePath, object obj);
    }
}
