using Generify.Models.Playlists;
using System.Collections.Generic;

namespace Generify.Models.Management
{
    public class User : Entity
    {
        public string UserNameInternal { get; set; }
        public string UserNameDisplay { get; set; }
        public byte[] PasswordHash { get; set; }
        public string AccessToken { get; set; }

        public List<PlaylistDefinition> PlaylistDefinitions { get; set; }
    }
}
