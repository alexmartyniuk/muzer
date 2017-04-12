package controllers

import (
	"bitbucket.org/Martinyuk/muzer/api/repositories"
	"strconv"
)

type ArtistDto struct {
	ID    uint   `json:"id"`
	Name  string `json:"name"`
	Thumb string `json:"thumb"`
}

type AlbumDto struct {
	ID    uint   `json:"id"`
	Title string `json:"title"`
	Year  uint   `json:"year"`
	Thumb string `json:"thumb"`
}

type TrackDto struct {
	ID       uint   `json:"id"`
	Title    string `json:"title"`
	Duration uint   `json:"duration"`
	Position string `json:"position"`
	URL      string `json:"url"`
	Size     string `json:"size"`
	Bitrate  string `json:"bitrate"`
}

type ArtistWithAlbumsDto struct {
	Artist *ArtistDto  `json:"artist"`
	Albums []*AlbumDto `json:"albums"`
}

type AlbumWithTracksDto struct {
	Album  *AlbumDto   `json:"album"`
	Tracks []*TrackDto `json:"tracks"`
}

func MakeArtistDto(artist *repositories.Artist) *ArtistDto {
	return &ArtistDto{
		artist.ID,
		artist.Name,
		artist.Thumb,
	}
}

func MakeAlbumDto(album *repositories.Album) *AlbumDto {
	return &AlbumDto{
		album.ID,
		album.Title,
		album.Year,
		album.Thumb,
	}
}

func MakeTrackDto(track *repositories.Track) *TrackDto {
	return &TrackDto{
		track.ID,
		track.Title,
		track.Duration,
		track.Position,
		"",
		"",
		"",
	}
}

func MakeArtistWithAlbumsDto(artist *repositories.Artist) *ArtistWithAlbumsDto {
	result := &ArtistWithAlbumsDto{}

	result.Artist = MakeArtistDto(artist)
	result.Albums = make([]*AlbumDto, len(artist.Albums))

	for i, album := range artist.Albums {
		result.Albums[i] = MakeAlbumDto(&album)
	}

	return result
}

func MakeAlbumWithTracksDto(album *repositories.Album) *AlbumWithTracksDto {
	result := &AlbumWithTracksDto{}

	result.Album = MakeAlbumDto(album)
	result.Tracks = make([]*TrackDto, len(album.Tracks))

	for i, track := range album.Tracks {
		result.Tracks[i] = MakeTrackDto(&track)
	}

	return result
}

func MakeTrackWithDownloadDto(track *repositories.Track, download *repositories.TrackDownload) *TrackDto {
	trackDto := MakeTrackDto(track)
	trackDto.Bitrate = strconv.Itoa(int(download.Bitrate))
	trackDto.Size = strconv.Itoa(int(download.Size))
	trackDto.URL = download.URL

	return trackDto
}
