using Generify.Models.Playlists;
using Generify.Repositories.Abstractions.Playlists;
using Generify.Services.Abstractions.Playlists;
using Generify.Services.Internal.Interfaces;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<PlaylistDefinition>> GetAllByUserIdAsync(string userId)
        {
            return await _playlistDefinitionRepo.GetAllByUserIdAsync(userId);
        }

        public async Task<PlaylistDefinition> GetByIdForUserAsync(string playlistId, string userId)
        {
            return await _playlistDefinitionRepo.GetByIdForUserAsync(playlistId, userId);
        }

        public async Task ExecuteGenerationAsync(string playlistDefinitionId)
        {
            PlaylistDefinition playlistDef = await _playlistDefinitionRepo.GetByIdAsync(playlistDefinitionId);

            if (playlistDef == null)
            {
                return;
            }

            await _playlistDefinitionRepo.LoadDetailsAsync(playlistDef);

            await _playlistGenerator.ExecuteGenerationAsync(playlistDef);
        }

        public async Task SaveAsync(PlaylistDefinition playlistDefinition)
        {
            playlistDefinition.PlaylistSources = playlistDefinition.PlaylistSources?
                .OrderBy(o => o.OrderNr)
                .Select((o, i) =>
                {
                    o.OrderNr = i + 1;

                    return o;
                })
                .ToList();

            playlistDefinition.OrderInstructions = playlistDefinition.OrderInstructions?
                .OrderBy(o => o.OrderNr)
                .Select((o, i) =>
                {
                    o.OrderNr = i + 1;

                    return o;
                })
                .ToList();

            await _playlistDefinitionRepo.SaveAsync(playlistDefinition);
        }
    }
}
