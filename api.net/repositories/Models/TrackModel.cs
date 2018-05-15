namespace MuzerAPI.Models
{
    public class TrackModel
    {
        public ulong Id { get; set; }
        public ulong AlbumId { get; set; }
        public string Position { get; set; }
        public string Title { get; set; }
        public ulong Duration { get; set; }

        public virtual AlbumModel Album { get; set; }
    }
}
