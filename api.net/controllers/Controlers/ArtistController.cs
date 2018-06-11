using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using MuzerAPI.DtoConvertors;
using MuzerAPI.Dtos;

namespace MuzerAPI.Controlers
{
    public class ArtistController : ApiController
    {
        private readonly ArtistService.ArtistService _artistService;

        public ArtistController(ArtistService.ArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet]
        [Route("artist/search")]
        public IEnumerable<ArtistDto> Search([FromUri] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var artists = _artistService.Search(query);

            return artists.Select(ArtistDtoConvertor.ArtistModelToDto);
        }

        [HttpGet]
        [Route("artist/{artistId}")]
        public ArtistWithAlbumsDto Get(long artistId)
        {
            if (artistId <= 0)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var artistWithAlbums = _artistService.GetByIdWithAlbums(artistId);
            
            return ArtistDtoConvertor.ArtistModelWithAlbumsToDto(artistWithAlbums); 
        }
    }
}