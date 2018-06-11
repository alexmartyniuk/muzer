using System;
using System.Collections.Generic;
using System.Linq;
using MuzerAPI.Models;
using MuzerAPI.Repositories;
using DiscogsNet.Api;
using DiscogsNet.Model.Search;

namespace MuzerAPI.ArtistService
{
    public class ArtistService
    {
        private readonly AlbumRepository _albumRepository;
        private readonly ArtistRepository _artistRepository;

        public ArtistService(AlbumRepository albumRepository, ArtistRepository artistRepository)
        {
            _albumRepository = albumRepository;
            _artistRepository = artistRepository;
        }

        public IEnumerable<ArtistModel> Search(string query)
        {
            var discogsClient = CreateDiscogsClient();
            var searchQuery = new SearchQuery
            {
                 Query = query,
                 Type = SearchItemType.Artist
            };

            var searchResults = discogsClient.Search(searchQuery).Results;
            var sourceIds = searchResults.Select(sr => sr.Id.ToString());

            var artistExisted = _artistRepository.GetBySourceIds(sourceIds);
            if (artistExisted.Count() == sourceIds.Count())
            {
                return artistExisted;
            }

            var sourceIdsExisting = artistExisted.Select(ar => ar.SourceId);
            var sourceIdsMissing = sourceIds.Where(si => !sourceIdsExisting.Contains(si));

            var artistsForSave = new List<ArtistModel>();
            foreach (var searchResult in searchResults.Where(sr => sourceIdsMissing.Contains(sr.Id.ToString())))
            {
                artistsForSave.Add(new ArtistModel
                {
                    Source = SourceType.Discogs,
                    SourceId = searchResult.Id.ToString(),
                    Name = searchResult.Title,
                    Thumb = searchResult.Thumb
                });                
            }

            _artistRepository.SaveMany(artistsForSave);

            return _artistRepository.GetBySourceIds(sourceIds);
        }

        public ArtistModel GetByIdWithAlbums(long artistId)
        {
            var artist = _artistRepository.GetByIdWithAlbums(artistId);
            if (artist == null)
            {
                throw new Exception($"Artist did not found by id: {artistId}");
            }

            if (string.IsNullOrEmpty(artist.Description))
            {
                var sourceId = int.Parse(artist.SourceId);
                DiscogsClient discogsClient = CreateDiscogsClient();
                var sourceArtist = discogsClient.GetArtist(sourceId);

                artist.Description = sourceArtist.Profile;
                _artistRepository.Update(artist);
            }

            if (!artist.Albums.Any())
            {
                var sourceId = int.Parse(artist.SourceId);
                var discogsClient = CreateDiscogsClient();

                var releasesForSave = new List<AlbumModel>();
                var sourceReleases = discogsClient.GetArtistReleases(sourceId);
                foreach (var sourceRelease in sourceReleases.Releases)
                {
                    releasesForSave.Add(new AlbumModel
                        {
                            ArtistId = artist.Id,
                            Source = SourceType.Discogs,
                            SourceId = sourceId.ToString(),
                            Thumb = sourceRelease.Thumb,
                            Title = sourceRelease.Title,
                            Year = sourceRelease.Year
                        }
                    );
                }

                _albumRepository.SaveMany(releasesForSave);
            }

            return _artistRepository.GetByIdWithAlbums(artistId);
        }

        private DiscogsClient CreateDiscogsClient()
        {
            return new DiscogsClient("HNmJpKxApdkwljeHZxXRMFGgGVMfsODoOJojIXfh", "Muzer API");
        }
    }
}
