using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscogsNet.Api;
using MuzerAPI.Models;
using MuzerAPI.Repositories;

namespace MuzerAPI.AlbumService
{
    public class AlbumService
    {
        private readonly AlbumRepository _albumRepository;
        private readonly TrackRepository _trackRepository;

        public AlbumService(AlbumRepository albumRepository, TrackRepository trackRepository)
        {
            _albumRepository = albumRepository;
            _trackRepository = trackRepository;
        }

        public AlbumModel GetByIdWithTracks(long albumId)
        {
            var album = _albumRepository.GetByIdWithTracks(albumId);
            if (album == null)
            {
                throw new Exception($"Artist did not found by id: {albumId}");
            }

            if (!album.Tracks.Any())
            {
                var sourceId = int.Parse(album.SourceId);
                var discogsClient = CreateDiscogsClient();

                var tracksForSave = new List<TrackModel>();
                var sourceTracks = discogsClient.GetRelease(sourceId);
                foreach (var sourceTrack in sourceTracks.Tracklist)
                {
                    tracksForSave.Add(new TrackModel
                    {
                        AlbumId = album.Id,
                        Title = sourceTrack.Title,
                        Duration = DurationToSec(sourceTrack.Duration),
                        Position = sourceTrack.Position                        
                    }
                    );
                }

                _trackRepository.SaveMany(tracksForSave);
            }

            return _albumRepository.GetByIdWithTracks(album.Id);
        }

        private ulong DurationToSec(string duration)
        {
            var min = ulong.Parse(duration.Split(':')[0]);
            var sec = ulong.Parse(duration.Split(':')[1]);

            return min * 60 + sec;
        }

        private DiscogsClient CreateDiscogsClient()
        {
            return new DiscogsClient("HNmJpKxApdkwljeHZxXRMFGgGVMfsODoOJojIXfh", "Muzer API");
        }
    }
}
