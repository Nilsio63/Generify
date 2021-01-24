using Generify.Services.Abstractions.Management;

namespace Generify.Services.Management
{
    public class ExternalAuthSettings : IExternalAuthSettings
    {
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string CallbackUrl { get; }

        public ExternalAuthSettings(string clientId, string clientSecret, string callbackUrl)
        {
            ClientId = clientId;
            CallbackUrl = callbackUrl;
            ClientSecret = clientSecret;
        }
    }
}
