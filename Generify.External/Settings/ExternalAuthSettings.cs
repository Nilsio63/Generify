using Generify.External.Abstractions.Settings;

namespace Generify.External.Settings;

public class ExternalAuthSettings(string clientId, string clientSecret, string callbackUrl) : IExternalAuthSettings
{
    public string ClientId { get; } = clientId;
    public string ClientSecret { get; } = clientSecret;
    public string CallbackUrl { get; } = callbackUrl;
}
