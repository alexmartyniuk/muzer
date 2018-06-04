using System.Collections.Generic;
using MuzerAPI.Models;

namespace MuzerAPI.Repositories
{
    public class TrackRepository
    {
        public void SaveMany(IEnumerable<TrackModel> tracks)
        {
            using (var database = new DatabaseContext())
            {
                database.Tracks.AddRange(tracks);
                database.SaveChanges();
            }
        }
    }
}
