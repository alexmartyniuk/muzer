using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MuzerAPI.Dtos
{
    public class TrackDataDto
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public ulong Duration { get; set; }
        public string Quality { get; set; }
        public string SourceUrl { get; set; }
    }
}