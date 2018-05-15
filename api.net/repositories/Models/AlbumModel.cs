namespace MuzerAPI.Models
{
    public class AlbumModel
    {
        public ulong ArtistId { get; set; }
        public SourceType Source { get; set; }
        public string SourceId { get; set; }
        public ulong Id { get; set; }
        public string Title { get; set; }
        public string Thumb { get; set; }
        public ulong Year { get; set; }

        public virtual ArtistModel Artist { get; set; }
    }
}