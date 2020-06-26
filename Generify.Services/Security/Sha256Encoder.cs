using Generify.Services.Interfaces.Security;
using System.Security.Cryptography;
using System.Text;

namespace Generify.Services.Security
{
    public class Sha256Encoder : IHashEncoder
    {
        public byte[] EncodeToHash(string str)
        {
            using SHA256 sha = SHA256.Create();

            byte[] bytes = Encoding.UTF8.GetBytes(str);

            return sha.ComputeHash(bytes);
        }
    }
}
