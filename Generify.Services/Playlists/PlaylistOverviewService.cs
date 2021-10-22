using Generify.Models.Management;
using Generify.Models.Playlists;
using Generify.Repositories.Abstractions.Management;
using Generify.Repositories.Abstractions.Playlists;
using Generify.Services.Abstractions.Playlists;
using Generify.Services.Internal.Interfaces;
using SpotifyAPI.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Generify.Services.Playlists
{
    public class PlaylistOverviewService : IPlaylistOverviewService
    {
        private readonly ISpotifyClientFactory _spotifyClientFactory;
        private readonly IUserRepository _userRepo;
        private readonly IPlaylistDefinitionRepository _playlistDefRepo;

        public PlaylistOverviewService(ISpotifyClientFactory spotifyClientFactory,
            IUserRepository userRepo,
            IPlaylistDefinitionRepository playlistDefRepo)
        {
            _spotifyClientFactory = spotifyClientFactory;
            _userRepo = userRepo;
            _playlistDefRepo = playlistDefRepo;
        }

        public async Task<List<PlaylistOverview>> GetAllByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new List<PlaylistOverview>();
            }

            User user = await _userRepo.GetByIdAsync(userId);

            List<PlaylistDefinition> definitions = await _playlistDefRepo.GetAllByUserIdAsync(user.Id);

            ISpotifyClient client = await _spotifyClientFactory.CreateClientAsync(user.RefreshToken);

            return await definitions
                .ToAsyncEnumerable()
                .SelectAwait(async o => new
                {
                    Def = o,
                    Playlist = await client.Playlists.Get(o.TargetPlaylistId)
                })
                .Select(o => new PlaylistOverview
                {
                    Definition = o.Def,
                    Name = o.Playlist.Name,
                    Description = HttpUtility.HtmlDecode(o.Playlist.Description)
                })
                .ToListAsync();
        }
    }
}
