namespace Generify.Services.Abstractions.Management
{
    public interface IExternalAuthSettings
    {
        string ClientId { get; }
        string CallbackUrl { get; }
    }
}
