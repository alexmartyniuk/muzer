using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MuzerAPI.Dtos;
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

        public static AlbumDto AlbumModelToDto(AlbumModel album)
        {
            return new AlbumDto
            {
                Id = album.Id,
                Title = album.Title,
                Year = album.Year,
                ThumbUrl = album.Thumb
            };
        }

        public static ArtistWithAlbumsDto ArtistModelWithAlbumsToDto(ArtistModel artist)
        {
            return new ArtistWithAlbumsDto
            {
                Artist = ArtistModelToDto(artist),
                Albums = artist.Albums?.Select(AlbumModelToDto)
            };
        }
    }
}