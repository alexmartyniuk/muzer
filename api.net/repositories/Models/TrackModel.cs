namespace MuzerAPI.Models
{
    public class TrackModel
    {
        public uint Id { get; set; }
        public uint AlbumId { get; set; }
        public string Position { get; set; }
        public string Title { get; set; }
        public uint Duration { get; set; }
    }
}
