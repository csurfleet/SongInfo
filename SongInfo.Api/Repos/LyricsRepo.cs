using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SongInfo.Api.Models;

namespace SongInfo.Api.Repos
{
    public interface ILyricsRepo
    {
        Task<string> GetLyrics(string artistName, string workName);
    }

    /// <summary>Allows us to cache any ILyricsRepo implementation.</summary>
    /// <remarks>
    /// Given more time, I would tie this to a real cache implementation. For this project it actually
    /// does not improve performance (as the cache will purge each time the CLI app is run) but shows
    /// my favoured method for caching repositories via an overlay class.
    /// </remarks>
    internal class CachedLyricsRepo : ILyricsRepo
    {
        private readonly ILyricsRepo _implementation;

        private Dictionary<(string, string), string> _cache = new Dictionary<(string, string), string>();

        public CachedLyricsRepo(ILyricsRepo implementation)
        {
            _implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
        }

        public async Task<string> GetLyrics(string artistName, string workName)
        {
            var key = (artistName.ToLower(), workName.ToLower());
            if (_cache.ContainsKey(key))
                return _cache[key];
            
            var lyrics = await _implementation.GetLyrics(artistName, workName);
            
            _cache.Add(key, lyrics);
            
            return lyrics;
        }
    }

    internal class LyricsOvhRepo : ILyricsRepo
    {
        private const string _apiUri = "http://api.lyrics.ovh/v1/";
        public async Task<string> GetLyrics(string artistName, string workName)
        {
            var searchString = _apiUri +
                System.Web.HttpUtility.UrlEncode(artistName) + "/" +
                System.Web.HttpUtility.UrlEncode(workName);
            var client = new HttpClient(); // HttpClients shouldn't be disposed
            client.Timeout = TimeSpan.FromSeconds(10);

            try
            {
                using (var result = await client.GetAsync(searchString))
                {
                    if (result.StatusCode != HttpStatusCode.OK)
                        return String.Empty;

                    var json = await result.Content.ReadAsStringAsync();
                    //Console.WriteLine(json);

                    var model = JsonSerializer.Deserialize<LyricSearchResult>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return model.Lyrics;
                }
            }
            catch
            {
                // This is horrible and in a real system would be instead joined into an exeption
                // reporting service, as well as handling the returned value in a way that degrades nicely
                return String.Empty;
            }
        }
    }
}