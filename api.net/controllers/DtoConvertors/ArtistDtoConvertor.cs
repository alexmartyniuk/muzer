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

        private static AlbumDto AlbumModelToDto(AlbumModel album)
        {
            return new AlbumDto
            {
                Id = album.Id,
                Title = album.Title,
                Year = album.Year,
                ThumbUrl = album.Thumb
            };
        }

        private static TrackDto TrackModelToDto(TrackModel track)
        {
            return new TrackDto
            {
                Id = track.Id,
                Position = track.Position,
                Title = track.Title,
                Data = TrackDataModelToDto(track.TrackDatas.FirstOrDefault())
            };
        }

        private static TrackDataDto TrackDataModelToDto(TrackDataModel trackData)
        {
            if (trackData == null)
            {
                return null;
            }

            var url = new UriBuilder(HttpContext.Current.Request.Url)
            {
                Port = 8080,
                Path = @"file/" + trackData.Url
            };

            return new TrackDataDto
            {
                Id = trackData.Id,
                Url = url.Uri.AbsoluteUri,
                Duration = trackData.Duration,
                Quality = trackData.Quality.ToString(),
                SourceUrl = trackData.SourceUrl
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

        public static ArtistWithTracksDto AlbumModelWithTracksToDto(AlbumModel album)
        {
            return new ArtistWithTracksDto
            {
                Album = AlbumModelToDto(album),
                Artist = ArtistModelToDto(album.Artist),
                Tracks = album.Tracks?.Select(TrackModelToDto)
            }; 
        }
    }
}