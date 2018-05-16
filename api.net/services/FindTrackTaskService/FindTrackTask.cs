using System;

namespace MuzerAPI.Implementation
{
    public class FindTrackTask
    {
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Track { get; set; }

        public override string ToString()
        {
            return $"{Artist} : {Album} - {Track}";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FindTrackTask) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Artist != null ? StringComparer.CurrentCulture.GetHashCode(Artist) : 0);
                hashCode = (hashCode * 397) ^ (Album != null ? StringComparer.CurrentCulture.GetHashCode(Album) : 0);
                hashCode = (hashCode * 397) ^ (Track != null ? StringComparer.CurrentCulture.GetHashCode(Track) : 0);
                return hashCode;
            }
        }

        protected bool Equals(FindTrackTask other)
        {
            return string.Equals(Artist, other.Artist, StringComparison.CurrentCulture) &&
                   string.Equals(Album, other.Album, StringComparison.CurrentCulture) &&
                   string.Equals(Track, other.Track, StringComparison.CurrentCulture);
        }
    }
}
