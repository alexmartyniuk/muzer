using System.Collections.Generic;
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
    }
}
