using Generify.Repositories.Interfaces;
using Generify.Repositories.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Repositories.Internal
{
    public class FileAccess : IFileAccess
    {
        public string GetBasePath()
        {
            return Directory.GetCurrentDirectory();
        }

        public IAsyncEnumerable<FileEntry<T>> ReadJsonFilesAsync<T>(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, "*.json")
                .ToAsyncEnumerable()
                .SelectAwait(async filePath =>
                {
                    string content = await File.ReadAllTextAsync(filePath);

                    return new
                    {
                        FilePath = filePath,
                        Content = content
                    };
                })
                .Select(file => new FileEntry<T>
                {
                    FilePath = file.FilePath,
                    Entity = JsonConvert.DeserializeObject<T>(file.Content)
                });
        }

        public Task WriteJsonAsync(string filePath, object obj)
        {
            throw new NotImplementedException();
        }
    }
}
