using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SongInfo.Api.Models;

namespace SongInfo.Api.Repos
{
    internal class MusicBrainzRepo
    {
        private const string _baseUri = "https://musicbrainz.org/ws/2/";
        public async Task<ArtistSearchResult> SearchArtists(string artistName)
        {
            var searchString = $"{_baseUri}artist?query=artist:%22{System.Web.HttpUtility.UrlEncode(artistName)}%22";
            var client = new HttpClient(); // HttpClients shouldn't be disposed
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "Chrises music search app v0.1");

            using (var result = await client.GetAsync(searchString))
            {
                var json = await result.Content.ReadAsStringAsync();
                //Console.WriteLine(json);

                var model = JsonSerializer.Deserialize<ArtistSearchResult>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return model;
            }
        }

        public async Task<Artist> GetArtist(Guid id)
        {
            var searchString = $"{_baseUri}artist/{id}?inc=works";
            var client = new HttpClient(); // HttpClients shouldn't be disposed
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "Chrises music search app v0.1");

            using (var result = await client.GetAsync(searchString))
            {
                var json = await result.Content.ReadAsStringAsync();
                //Console.WriteLine(json);

                var model = JsonSerializer.Deserialize<Artist>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return model;
            }
        }
    }
}