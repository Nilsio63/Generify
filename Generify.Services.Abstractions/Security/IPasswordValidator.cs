namespace Generify.Services.Abstractions.Security
{
    public interface IPasswordValidator
    {
        string ValidatePassword(string password);
    }
}
