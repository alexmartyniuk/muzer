using System.Collections.Generic;

namespace MuzerAPI.Models
{
    public class AlbumModel
    {
        public long ArtistId { get; set; }
        public SourceType Source { get; set; }
        public string SourceId { get; set; }
        public long Id { get; set; }
        public string Title { get; set; }
        public string Thumb { get; set; }
        public long Year { get; set; }
        public virtual ArtistModel Artist { get; set; }
        public ICollection<TrackModel> Tracks { get; set; }
    }
}