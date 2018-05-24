using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuzerAPI.Models;

namespace MuzerAPI.Repositories
{
    public class ArtistRepository
    {
        public ArtistModel GetBySourceId(string sourceId)
        {
            using (var database = new DatabaseContext())
            {
                return database.Artists.SingleOrDefault(art => art.SourceId == sourceId);
            }
        }

        public IList<ArtistModel> GetBySourceIds(IEnumerable<string> sourceIds)
        {
            using (var database = new DatabaseContext())
            {
                return database.Artists.Where(art => sourceIds.Contains(art.SourceId)).ToList();
            }
        }

        public void SaveOne(ArtistModel artistNew)
        {
            using (var database = new DatabaseContext())
            {
                database.Artists.Add(artistNew);
                database.SaveChanges();
            }
        }

        public void SaveMany(IEnumerable<ArtistModel> artistsNew)
        {
            using (var database = new DatabaseContext())
            {
                database.Artists.AddRange(artistsNew);
                database.SaveChanges();
            }
        }
    }
}
