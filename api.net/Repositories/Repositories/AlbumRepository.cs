using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
