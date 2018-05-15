namespace MuzerAPI.Models
{
    public class TrackDataModel
    {
        public ulong Id { get; set; }
        public ulong TrackId { get; set; }
        public ulong Duration { get; set; }
        public ulong Quality { get; set; }
        public ulong Relevance { get; set; }
        public string Url { get; set; }
        public SourceType Source { get; set; }
        public string SourceId { get; set; }
        public string SourceUrl { get; set; }

        public virtual TrackModel Track { get; set; }
    }
}
