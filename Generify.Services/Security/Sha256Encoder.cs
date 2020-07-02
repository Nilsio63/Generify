using Generify.Services.Interfaces.Security;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Generify.Services.Security
{
    public class Sha256Encoder : IHashEncoder
    {
        private readonly ISaltSettings _saltSettings;

        public Sha256Encoder(ISaltSettings saltSettings)
        {
            _saltSettings = saltSettings;
        }

        public byte[] EncodeToHash(string str)
        {
            using SHA256 sha = SHA256.Create();

            byte[] bytes = Encoding.UTF8.GetBytes(str)
                .Concat(_saltSettings.PasswordSalt)
                .ToArray();

            return sha.ComputeHash(bytes);
        }
    }
}
