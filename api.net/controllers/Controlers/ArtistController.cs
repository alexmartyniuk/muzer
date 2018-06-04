using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using MuzerAPI.ArtistService;
using MuzerAPI.DtoConvertors;
using MuzerAPI.Dtos;

namespace MuzerAPI.Controllers
{
    public class ArtistController : ApiController
    {
        [HttpGet]
        [Route("artist/search")]
        public IEnumerable<ArtistDto> Search([FromUri] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var service = new ArtistService.ArtistService();
            var artists = service.Search(query);

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

            var service = new ArtistService.ArtistService();
            var artistWithAlbums = service.GetByIdWithAlbums(artistId);
            
            return ArtistDtoConvertor.ArtistModelWithAlbumsToDto(artistWithAlbums); 
        }
    }
}