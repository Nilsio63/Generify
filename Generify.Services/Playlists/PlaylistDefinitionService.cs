using Generify.Models.Playlists;
using Generify.Repositories.Abstractions.Playlists;
using Generify.Services.Abstractions.Playlists;
using Generify.Services.Internal.Interfaces;
using System.Threading.Tasks;

namespace Generify.Services.Playlists
{
    public class PlaylistDefinitionService : IPlaylistDefinitionService
    {
        private readonly IPlaylistGenerator _playlistGenerator;
        private readonly IPlaylistDefinitionRepository _playlistDefinitionRepo;

        public PlaylistDefinitionService(IPlaylistGenerator playlistGenerator,
            IPlaylistDefinitionRepository playlistDefinitionRepo)
        {
            _playlistGenerator = playlistGenerator;
            _playlistDefinitionRepo = playlistDefinitionRepo;
        }

        public async Task ExecuteGenerationAsync(string playlistDefinitionId)
        {
            PlaylistDefinition playlistDef = await _playlistDefinitionRepo.GetByIdAsync(playlistDefinitionId);

            if (playlistDef == null)
            {
                return;
            }

            await _playlistGenerator.ExecuteGenerationAsync(playlistDef);
        }
    }
}
