using Generify.External.Abstractions.Services;
using Generify.Models.Management;
using Generify.Models.Playlists;
using Generify.Repositories.Abstractions.Management;
using Generify.Repositories.Abstractions.Playlists;
using Generify.Services.Abstractions.Playlists;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Services.Playlists
{
    public class PlaylistOverviewService : IPlaylistOverviewService
    {
        private readonly IPlaylistInfoService _playlistInfoService;
        private readonly IUserRepository _userRepo;
        private readonly IPlaylistDefinitionRepository _playlistDefRepo;

        public PlaylistOverviewService(IPlaylistInfoService playlistInfoService,
            IUserRepository userRepo,
            IPlaylistDefinitionRepository playlistDefRepo)
        {
            _playlistInfoService = playlistInfoService;
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

            return await definitions
                .ToAsyncEnumerable()
                .SelectAwait(async o => new
                {
                    Def = o,
                    Playlist = await _playlistInfoService.GetPlaylistInfoAsync(o.TargetPlaylistId)
                })
                .Select(o => new PlaylistOverview
                {
                    Definition = o.Def,
                    Name = o.Playlist.Name,
                    Description = o.Playlist.Description
                })
                .ToListAsync();
        }
    }
}
