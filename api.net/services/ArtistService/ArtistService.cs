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
        public IEnumerable<ArtistModel> Search(string query)
        {
            var discogsClient = CreateDiscogsClient();
            var searchQuery = new SearchQuery
            {
                 Query = query,
                 Type = SearchItemType.Artist
            };

            var artistRepository = new ArtistRepository();
            var searchResults = discogsClient.Search(searchQuery).Results;
            var sourceIds = searchResults.Select(sr => sr.Id.ToString());

            var artistExisted = artistRepository.GetBySourceIds(sourceIds);
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

            artistRepository.SaveMany(artistsForSave);

            return artistRepository.GetBySourceIds(sourceIds);
        }

        public ArtistModel GetByIdWithAlbums(long artistId)
        {
            var artistRepository = new ArtistRepository();
            var albumRepository = new AlbumRepository();

            var artist = artistRepository.GetByIdWithAlbums(artistId);
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
                artistRepository.Update(artist);
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

                albumRepository.SaveMany(releasesForSave);
            }

            return artistRepository.GetByIdWithAlbums(artistId);
        }

        private DiscogsClient CreateDiscogsClient()
        {
            return new DiscogsClient("HNmJpKxApdkwljeHZxXRMFGgGVMfsODoOJojIXfh", "Muzer API");
        }
    }
}
