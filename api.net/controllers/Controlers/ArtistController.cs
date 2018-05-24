using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using MuzerAPI.ArtistService;
using MuzerAPI.DtoConvertors;

namespace MuzerAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class ArtistController : ApiController
    {
        [HttpGet]
        [Route("artist/search")]
        [EnableCors("http://localhost:3000", null, "GET")]
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
    }
}