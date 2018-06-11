using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using MuzerAPI.AlbumService;
using MuzerAPI.DtoConvertors;
using MuzerAPI.Dtos;

namespace MuzerAPI.Controlers
{
    public class AlbumController : ApiController
    {
        private readonly AlbumService.AlbumService _albumService;

        public AlbumController(AlbumService.AlbumService albumService)
        {
            _albumService = albumService;            
        }

        [HttpGet]
        [Route("album/{albumId}")]
        public ArtistWithTracksDto Get(long albumId)
        {
            if (albumId <= 0)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var artistWithAlbums = _albumService.GetByIdWithTracks(albumId);

            return ArtistDtoConvertor.AlbumModelWithTracksToDto(artistWithAlbums);
        }
    }
}