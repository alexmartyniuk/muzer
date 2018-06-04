using System.Collections.Generic;

namespace MuzerAPI.Models
{
    public class TrackModel
    {
        public long Id { get; set; }
        public long AlbumId { get; set; }
        public string Position { get; set; }
        public string Title { get; set; }
        public ulong Duration { get; set; }

        public virtual AlbumModel Album { get; set; }
        public ICollection<TrackDataModel> TrackDatas { get; set; }
    }
}
