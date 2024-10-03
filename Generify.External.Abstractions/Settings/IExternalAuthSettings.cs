namespace Generify.External.Abstractions.Settings;

public interface IExternalAuthSettings
{
    string ClientId { get; }
    string ClientSecret { get; }
    string CallbackUrl { get; }
}
