namespace Generify.Services.Interfaces.Security
{
    public interface ISaltSettings
    {
        byte[] PasswordSalt { get; }
    }
}
