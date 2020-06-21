using Generify.Models.Playlists;
using System.Collections.Generic;

namespace Generify.Models.Management
{
    public class User : Entity
    {
        public string AccountName { get; set; }
        public byte[] PasswordHash { get; set; }
        public string AccessToken { get; set; }

        public List<PlaylistDefinition> PlaylistDefinitions { get; set; }
    }
}
