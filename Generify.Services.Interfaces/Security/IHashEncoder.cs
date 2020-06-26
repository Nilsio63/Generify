namespace Generify.Services.Interfaces.Security
{
    public interface IHashEncoder
    {
        byte[] EncodeToHash(string str);
    }
}
