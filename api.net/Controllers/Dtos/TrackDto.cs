using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MuzerAPI.Dtos
{
    public class TrackDto
    {
        public long Id { get; set; }
        public string Position { get; set; }
        public string Title { get; set; }
        public TrackDataDto Data { get; set; }
    }
}