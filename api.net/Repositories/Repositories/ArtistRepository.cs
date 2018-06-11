using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuzerAPI.Models;

namespace MuzerAPI.Repositories
{
    public class ArtistRepository
    {
        private readonly DatabaseContext _database;

        public ArtistRepository(DatabaseContext database)
        {
            _database = database;
        }

        public ArtistModel GetBySourceId(string sourceId)
        {
            return _database.Artists.SingleOrDefault(art => art.SourceId == sourceId);
        }

        public IList<ArtistModel> GetBySourceIds(IEnumerable<string> sourceIds)
        {
            return _database.Artists.Where(art => sourceIds.Contains(art.SourceId)).ToList();
        }

        public void SaveOne(ArtistModel artistNew)
        {
            _database.Artists.Add(artistNew);
            _database.SaveChanges();            
        }

        public void SaveMany(IEnumerable<ArtistModel> artistsNew)
        {
            _database.Artists.AddRange(artistsNew);
            _database.SaveChanges();
        }

        public ArtistModel GetByIdWithAlbums(long artistId)
        {
            return _database.Artists
                .Include(ar => ar.Albums)
                .SingleOrDefault(ar => ar.Id == artistId);                            
        }

        public ArtistModel GetById(long artistId)
        {
            return _database.Artists
                .SingleOrDefault(ar => ar.Id == artistId);            
        }

        public void Update(ArtistModel artist)
        {
            _database.Artists.AddOrUpdate(artist);
            _database.SaveChanges();            
        }
    }
}
