using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SongInfo.Api.Models;
using SongInfo.Api.Repos;

namespace SongInfo.Api
{
    public class ArtistCommandRunner
    {
        private readonly ILyricsRepo _lyricsRepo = new CachedLyricsRepo(new LyricsOvhRepo());

        public async Task<List<string>> RunCommand(string command)
        {
            var musicRepo = new MusicBrainzRepo();
            var results = await musicRepo.SearchArtists(command);

            if (results.Count > 1)
            {
                var artistList = new List<string>
                {
                    $"Found {results.Count} artists. Refine your search to a single artist to see their details",
                    "Artists found:"
                };
                
                artistList.AddRange(results.Artists.Select(a => a.Name));
                return artistList;
            }

            if (results.Count < 1)
                return new List<string> { $"Found no artists, please refine your search " };

            var artist = await musicRepo.GetArtist(results.Artists[0].Id);

            return new List<string>
                {
                    $"Name: {artist.Name}",
                    $"Id: {artist.Id}",
                    $"No of works: {artist.Works.Count}",
                    $"Total number of words in all works: {await GetTotalWordCount(artist)}"
                };
        }

        private async Task<int> GetTotalWordCount(Artist artist)
        {
            // Filter out any duplicates, just in case the dataset isn't quite right
            var works = artist.Works
                .Select(w => w.Title)
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct();

            // Now we have the titles themselves we need to call the other service to get the words for each song
            var wordCounts = new List<int>();
            foreach(var work in works) // TODO: Opportunity here for multi-threading
            {
                var lyrics = await _lyricsRepo.GetLyrics(artist.Name, work);
                wordCounts.Add(GetWordCount(lyrics));
            }

            return wordCounts.Sum();
        } 

        private int GetWordCount(string input)
        {
            // This is a standard word count alg stolen from the internet

            if (input == null)
                return 0;

            int wordCount = 0, index = 0;

            while (index < input.Length && char.IsWhiteSpace(input[index]))
                index++;

            while (index < input.Length)
            {
                while (index < input.Length && !char.IsWhiteSpace(input[index]))
                    index++;

                wordCount++;

                while (index < input.Length && char.IsWhiteSpace(input[index]))
                    index++;
            }

            return wordCount;
        }
    }
}