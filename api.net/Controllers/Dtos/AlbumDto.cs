using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MuzerAPI.Dtos
{
    public class AlbumDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long Year { get; set; }
        public string ThumbUrl { get; set; }
    }
}