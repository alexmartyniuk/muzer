namespace MuzerAPI.Models
{
    public class AlbumModel
    {
        public uint ArtistId { get; set; }
        public SourceType Source { get; set; }
        public uint SourceId { get; set; }
        public uint Id { get; set; }
        public string Title { get; set; }
        public string Thumb { get; set; }
        public uint Year { get; set; }        
    }
}