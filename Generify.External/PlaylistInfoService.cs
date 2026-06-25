using Generify.External.Abstractions.Models;
using Generify.External.Abstractions.Services;
using Generify.External.Internal.Interfaces;
using SpotifyAPI.Web;
using System.Web;

namespace Generify.External;

public class PlaylistInfoService(ISpotifyClientFactory spotifyClientFactory) : IPlaylistInfoService
{
    public async Task<PlaylistInfo?> GetPlaylistInfoAsync(string playlistId)
    {
        ISpotifyClient client = await spotifyClientFactory.CreateClientAsync();

        FullPlaylist playlist = await client.Playlists.Get(playlistId);

        return playlist is null
            ? null
            : new PlaylistInfo
            {
                Id = playlist.Id!,
                Name = playlist.Name!,
                Description = HttpUtility.HtmlDecode(playlist.Description),
                ImageUrl = playlist.Images?.OrderByDescending(o => o.Height).FirstOrDefault()?.Url
            };
    }
}
