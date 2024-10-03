using Generify.External.Abstractions.Models;
using Generify.External.Abstractions.Services;
using Generify.External.Internal.Interfaces;
using SpotifyAPI.Web;
using System.Threading.Tasks;
using System.Web;

namespace Generify.External;

public class PlaylistInfoService : IPlaylistInfoService
{
    private readonly ISpotifyClientFactory _spotifyClientFactory;

    public PlaylistInfoService(ISpotifyClientFactory spotifyClientFactory)
    {
        _spotifyClientFactory = spotifyClientFactory;
    }

    public async Task<PlaylistInfo?> GetPlaylistInfoAsync(string playlistId)
    {
        ISpotifyClient client = await _spotifyClientFactory.CreateClientAsync();

        FullPlaylist playlist = await client.Playlists.Get(playlistId);

        return playlist is null
            ? null
            : new PlaylistInfo
            {
                Id = playlist.Id!,
                Name = playlist.Name!,
                Description = HttpUtility.HtmlDecode(playlist.Description)
            };
    }
}
