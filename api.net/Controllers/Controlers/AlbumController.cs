using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using MuzerAPI.ArtistService;
using MuzerAPI.DtoConvertors;
using MuzerAPI.Dtos;

namespace MuzerAPI.Controlers
{
    public class AlbumController : ApiController
    {
        [HttpGet]
        [Route("album/{albumId}")]
        public ArtistWithTracksDto Get(long albumId)
        {
            if (albumId <= 0)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var service = new AlbumService.AlbumService();
            var artistWithAlbums = service.GetByIdWithTracks(albumId);

            return ArtistDtoConvertor.AlbumModelWithTracksToDto(artistWithAlbums);
        }
    }
}