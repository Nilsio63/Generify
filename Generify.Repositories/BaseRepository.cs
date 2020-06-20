using Generify.Repositories.Interfaces;
using Generify.Repositories.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Repositories
{
    public abstract class BaseRepository<T>
    {
        private readonly IFileAccess _fileAccess;

        protected abstract string FolderName { get; }

        public BaseRepository(IFileAccess fileAccess)
        {
            _fileAccess = fileAccess;
        }

        protected IAsyncEnumerable<T> ReadEntitiesAsync()
        {
            return ReadFilesAsync().Select(o => o.Entity);
        }

        protected IAsyncEnumerable<FileEntry<T>> ReadFilesAsync()
        {
            string directoryPath = GetDirectoryPath();

            return _fileAccess.ReadJsonFilesAsync<T>(directoryPath);
        }

        protected async Task WriteEntityAsync(T entity, Func<T, bool> matchingEntryPredicate)
        {
            FileEntry<T> match = await ReadFilesAsync()
                .Where(o => matchingEntryPredicate(o.Entity))
                .FirstOrDefaultAsync();

            string filePath = match?.FilePath
                ?? Path.Combine(GetDirectoryPath(), Guid.NewGuid() + ".json");

            await _fileAccess.WriteJsonAsync(filePath, entity);
        }

        private string GetDirectoryPath()
        {
            return Path.Combine(_fileAccess.GetBasePath(), "data", FolderName);
        }
    }
}
