using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MuzerAPI.Dtos
{
    public class ArtistWithAlbumsDto
    {
        public ArtistDto Artist { get; set; }
        public IEnumerable<AlbumDto> Albums { get; set; }
    }
}