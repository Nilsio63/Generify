namespace Generify.Services.Abstractions.Security
{
    public interface ISaltSettings
    {
        byte[] PasswordSalt { get; }
    }
}
