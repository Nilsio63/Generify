using Generify.Services.Abstractions.Management;

namespace Generify.Services.Management
{
    public class ExternalAuthSettings : IExternalAuthSettings
    {
        public string ClientId { get; }
        public string CallbackUrl { get; }

        public ExternalAuthSettings(string clientId, string callbackUrl)
        {
            ClientId = clientId;
            CallbackUrl = callbackUrl;
        }
    }
}
