using System.Collections.Generic;

namespace SongInfo.Api.Models
{
    public class ArtistSearchResult
    {
        public int Count { get; set; }

        public List<Artist> Artists { get; set; }
    }
}