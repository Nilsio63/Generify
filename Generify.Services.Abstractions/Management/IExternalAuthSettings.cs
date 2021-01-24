namespace Generify.Services.Abstractions.Management
{
    public interface IExternalAuthSettings
    {
        string ClientId { get; }
        string ClientSecret { get; }
        string CallbackUrl { get; }
    }
}
