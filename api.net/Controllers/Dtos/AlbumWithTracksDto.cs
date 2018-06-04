using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MuzerAPI.Dtos
{
    public class ArtistWithTracksDto
    {
        public AlbumDto Album { get; set; }
        public ArtistDto Artist { get; set; }
        public IEnumerable<TrackDto> Tracks { get; set; }
    }
}