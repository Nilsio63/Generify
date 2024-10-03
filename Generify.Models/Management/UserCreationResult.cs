namespace Generify.Models.Management;

public class UserCreationResult
{
    public bool IsSuccess => string.IsNullOrWhiteSpace(ErrorMessage);
    public User CreatedUser { get; }
    public string ErrorMessage { get; }

    public UserCreationResult(User createdUser)
    {
        CreatedUser = createdUser;
        ErrorMessage = null;
    }

    public UserCreationResult(string errorMessage)
    {
        ErrorMessage = errorMessage;
        CreatedUser = null;
    }
}
