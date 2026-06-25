using Generify.Models.Playlists;
using Generify.Repositories.Abstractions.Playlists;
using Generify.Services.Abstractions.Playlists;
using Generify.Services.Internal.Interfaces;

namespace Generify.Services.Playlists;

public class PlaylistDefinitionService(
    IPlaylistGenerator playlistGenerator,
    IPlaylistDefinitionRepository playlistDefinitionRepo)
    : IPlaylistDefinitionService
{
    public async Task<List<PlaylistDefinition>> GetAllByUserIdAsync(string userId)
    {
        return await playlistDefinitionRepo.GetAllByUserIdAsync(userId);
    }

    public async Task<PlaylistDefinition?> GetByIdForUserAsync(string playlistId, string userId)
    {
        return await playlistDefinitionRepo.GetByIdForUserAsync(playlistId, userId);
    }

    public async Task LoadDetailsAsync(PlaylistDefinition playlistDefinition)
    {
        await playlistDefinitionRepo.LoadDetailsAsync(playlistDefinition);
    }

    public async Task ExecuteGenerationAsync(string playlistDefinitionId)
    {
        PlaylistDefinition? playlistDef = await playlistDefinitionRepo.GetByIdAsync(playlistDefinitionId);

        if (playlistDef == null)
        {
            return;
        }

        await playlistDefinitionRepo.LoadDetailsAsync(playlistDef);

        await playlistGenerator.ExecuteGenerationAsync(playlistDef);
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
            .ToList() ?? [];

        playlistDefinition.OrderInstructions = playlistDefinition.OrderInstructions?
            .OrderBy(o => o.OrderNr)
            .Select((o, i) =>
            {
                o.OrderNr = i + 1;

                return o;
            })
            .ToList() ?? [];

        await playlistDefinitionRepo.SaveAsync(playlistDefinition);
    }

    public async Task DeleteAsync(PlaylistDefinition playlistDefinition)
    {
        await playlistDefinitionRepo.DeleteAsync(playlistDefinition);
    }
}
