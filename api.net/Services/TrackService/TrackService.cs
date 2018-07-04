using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuzerAPI.Models;
using MuzerAPI.Repositories;

namespace MuzerAPI.TrackService
{
    public class TrackService
    {
        private TrackRepository _trackRepository;

        public TrackService(TrackRepository trackRepository)
        {
            _trackRepository = trackRepository;
        }

        public TrackModel GetTrackWithArtistById(long trackId)
        {
            return _trackRepository.GetTrackWithArtistById(trackId);
        }
    }
}
