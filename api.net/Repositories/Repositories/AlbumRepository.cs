using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using MuzerAPI.Models;

namespace MuzerAPI.Repositories
{
    public class AlbumRepository
    {
        public void SaveMany(IEnumerable<AlbumModel> albumsNew)
        {
            using (var database = new DatabaseContext())
            {
                database.Albums.AddRange(albumsNew);
                database.SaveChanges();
            }
        }

        public AlbumModel GetByIdWithTracks(long albumId)
        {
            using (var database = new DatabaseContext())
            {
                return database.Albums
                    .Include(al => al.Artist)
                    .Include(al => al.Tracks)
                    .Include(al => al.Tracks.Select(tr => tr.TrackDatas))
                    .SingleOrDefault(al => al.Id == albumId);
            }
        }
    }
}
