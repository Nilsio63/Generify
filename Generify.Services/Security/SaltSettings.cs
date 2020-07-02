using Generify.Services.Interfaces.Security;
using System.Text;

namespace Generify.Services.Security
{
    public class SaltSettings : ISaltSettings
    {
        public byte[] PasswordSalt { get; set; }

        public SaltSettings(string saltString)
        {
            PasswordSalt = !string.IsNullOrWhiteSpace(saltString)
                ? Encoding.UTF8.GetBytes(saltString)
                : new byte[0];
        }
    }
}
