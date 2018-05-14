namespace MuzerAPI.Models
{
    public class ArtistModel
    {
        public SourceType Source { get; set; }
        public long SourceId { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Thumb { get; set; }
    }
}