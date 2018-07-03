namespace MuzerAPI.Models
{
    public class TrackDataModel
    {
        public long Id { get; set; }
        public long TrackId { get; set; }
        public long Duration { get; set; }
        public long Quality { get; set; }
        public long Relevance { get; set; }
        public string Url { get; set; }
        public SourceType Source { get; set; }
        public string SourceId { get; set; }
        public string SourceUrl { get; set; }

        public virtual TrackModel Track { get; set; }
    }
}
