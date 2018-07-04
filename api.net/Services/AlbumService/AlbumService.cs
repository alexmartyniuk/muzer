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
        private readonly FindTrackTaskService _findTrackService;

        public AlbumService(AlbumRepository albumRepository, TrackRepository trackRepository, FindTrackTaskService findTrackService)
        {
            _albumRepository = albumRepository;
            _trackRepository = trackRepository;
            _findTrackService = findTrackService;
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
                var sourceTracks = discogsClient.GetMasterRelease(sourceId);
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

            var result = _albumRepository.GetByIdWithTracks(album.Id);

            // create find-tasks for tracks with empty data
            foreach (var trackModel in result.Tracks)
            {
                if (!trackModel.TrackDatas.Any())
                {
                    var task = _findTrackService.NewTask(trackModel);
                    _findTrackService.AddTask(task);
                }
            }

            return result;
        }

        private long DurationToSec(string duration)
        {
            if (string.IsNullOrEmpty(duration))
            {
                return 0;
            }

            var min = long.Parse(duration.Split(':')[0]);
            var sec = long.Parse(duration.Split(':')[1]);

            return min * 60 + sec;
        }

        private DiscogsClient CreateDiscogsClient()
        {
            return new DiscogsClient("HNmJpKxApdkwljeHZxXRMFGgGVMfsODoOJojIXfh", "Muzer API");
        }
    }
}
