namespace Generify.Services.Interfaces.Security
{
    public interface IPasswordValidator
    {
        string ValidatePassword(string password);
    }
}
