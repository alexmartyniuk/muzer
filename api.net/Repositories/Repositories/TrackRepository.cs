using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MuzerAPI.Models;

namespace MuzerAPI.Repositories
{
    public class TrackRepository
    {
        private readonly DatabaseContext _database;

        public TrackRepository(DatabaseContext database)
        {
            _database = database;
        }
        public void SaveMany(IEnumerable<TrackModel> tracks)
        {
            _database.Tracks.AddRange(tracks);
            _database.SaveChanges();            
        }

        public TrackModel GetTrackWithArtistById(long trackId)
        {
            return _database.Tracks
                .Include(tr => tr.Album)
                .Include(tr => tr.Album.Artist)
                .SingleOrDefault(tr => tr.Id == trackId);
        }
    }
}
