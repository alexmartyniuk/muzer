namespace MuzerAPI.Models
{
    public class TrackDataModel
    {
        public uint Id { get; set; }
        public uint TrackId { get; set; }
        public uint Duration { get; set; }
        public uint Quality { get; set; }
        public uint Relevance { get; set; }
        public string Url { get; set; }
        public SourceType Source { get; set; }
        public uint SourceId { get; set; }
        public string SourceUrl { get; set; }
    }
}
