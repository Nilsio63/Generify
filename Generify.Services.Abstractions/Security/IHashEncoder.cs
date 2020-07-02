namespace Generify.Services.Abstractions.Security
{
    public interface IHashEncoder
    {
        byte[] EncodeToHash(string str);
    }
}
