using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using MuzerAPI.ArtistService;
using MuzerAPI.DtoConvertors;

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
    }
}