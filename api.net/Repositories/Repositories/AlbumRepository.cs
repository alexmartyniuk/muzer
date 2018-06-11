using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using MuzerAPI.Models;

namespace MuzerAPI.Repositories
{
    public class AlbumRepository
    {
        private readonly DatabaseContext _database;

        public AlbumRepository(DatabaseContext database)
        {
            _database = database;
        }

        public void SaveMany(IEnumerable<AlbumModel> albumsNew)
        {
            _database.Albums.AddRange(albumsNew);
            _database.SaveChanges();            
        }

        public AlbumModel GetByIdWithTracks(long albumId)
        {
            return _database.Albums
                .Include(al => al.Artist)
                .Include(al => al.Tracks)
                .Include(al => al.Tracks.Select(tr => tr.TrackDatas))
                .SingleOrDefault(al => al.Id == albumId);            
        }
    }
}
