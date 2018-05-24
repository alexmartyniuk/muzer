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
            var discogsClient = new DiscogsClient("HNmJpKxApdkwljeHZxXRMFGgGVMfsODoOJojIXfh", "Muzer API");
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
    }
}
