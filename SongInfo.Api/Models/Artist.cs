using System;
using System.Collections.Generic;

namespace SongInfo.Api.Models
{
    public class Artist
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public List<Work> Works { get; set; }

    }
}