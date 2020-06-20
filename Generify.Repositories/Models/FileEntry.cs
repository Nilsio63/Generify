namespace Generify.Repositories.Models
{
    public class FileEntry<T>
    {
        public string FilePath { get; set; }
        public T Entity { get; set; }
    }
}
