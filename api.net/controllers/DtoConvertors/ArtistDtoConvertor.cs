using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MuzerAPI.Models;

namespace MuzerAPI.DtoConvertors
{
    public static class ArtistDtoConvertor
    {
        public static ArtistDto ArtistModelToDto(ArtistModel artist)
        {
            return new ArtistDto
            {
                Id = artist.Id,
                Name = artist.Name,
                Descrition = artist.Description,
                ThumbUrl = artist.Thumb
            };
        }
    }
}