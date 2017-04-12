package controllers

import (
	"bitbucket.org/Martinyuk/muzer/api/services"
	"fmt"
	"github.com/labstack/echo"
	"net/http"
	"strconv"
)

// FindArtists perform search for artists by search term.
func FindArtists(context echo.Context) error {
	// 1. Check input params.
	query := context.QueryParam("query")
	if query == "" {
		return echo.NewHTTPError(http.StatusBadRequest, "Query not specified.")
	}

	// 2. Find artists and save them into DB.
	c := context.(*Context)
	artists := services.SearchAndSaveArtistsByTerm(c.Transaction, query)

	// 3. Construct DTO for saved artist models and add them to the result list.
	artistDtoList := make([]ArtistDto, len(artists))
	for i, artist := range artists {
		artistDtoList[i] = ArtistDto{
			ID:    artist.ID,
			Name:  artist.Name,
			Thumb: artist.Thumb,
		}
	}

	// 4. Return responce with list of artitsts as JSON.
	return context.JSON(http.StatusOK, artistDtoList)
}

// GetArtistDetails get artists with all details ant tracks list by artist_id.
func GetArtistDetails(context echo.Context) error {
	// 1. Check input params.
	artistID, err := strconv.ParseUint(context.Param("artist_id"), 0, 32)
	if err != nil {
		return echo.NewHTTPError(http.StatusBadRequest, "artist_id not specified or has incorect format.")
	}

	// 2. Get artist details by ID.
	c := context.(*Context)
	artist := services.GetArtistWithAlbumsByID(c.Transaction, uint(artistID))
	if artist == nil {
		return echo.NewHTTPError(http.StatusNotFound, fmt.Sprintf("Artist with id '%d' not found.", artistID))
	}

	// 3. Create DTO from list of albums and return responce as JSON.
	resultDto := MakeArtistWithAlbumsDto(artist)
	return context.JSON(http.StatusOK, resultDto)
}

// GetAlbumDetails get album with all tracks by album_id.
func GetAlbumDetails(context echo.Context) error {
	// 1. Check input params.
	albumID, err := strconv.ParseUint(context.Param("album_id"), 0, 32)
	if err != nil {
		return echo.NewHTTPError(http.StatusBadRequest, "album_id is not specified or has incorect format.")
	}

	// 2. Get album with tracks by ID.
	c := context.(*Context)
	album := services.GetAlbumWithTracksByID(c.Transaction, uint(albumID))
	if album == nil {
		return echo.NewHTTPError(http.StatusNotFound, fmt.Sprintf("Album with id '%d' not found.", albumID))
	}

	// 3. Create DTO and return responce as JSON.
	resultDto := MakeAlbumWithTracksDto(album)
	return context.JSON(http.StatusOK, resultDto)
}

// GetTrackDetails return track with all information including URL for downloading by track_id.
func GetTrackDetails(context echo.Context) error {
	// 1. Check input params.
	trackID, err := strconv.ParseUint(context.Param("track_id"), 0, 32)
	if err != nil {
		return echo.NewHTTPError(http.StatusBadRequest, "track_id is not specified or has incorect format.")
	}

	// 2. Get album with tracks by ID.
	c := context.(*Context)
	track, download := services.GetTrackWithDownloadByID(c.Transaction, uint(trackID))
	if track == nil {
		return echo.NewHTTPError(http.StatusNotFound, fmt.Sprintf("Track with id '%d' not found.", trackID))
	}
	if download == nil {
		return echo.NewHTTPError(http.StatusNotFound, fmt.Sprintf("Download URL for track '%d' not found.", trackID))
	}

	// 3. Create DTO and return responce as JSON.
	resultDto := MakeTrackWithDownloadDto(track, download)
	return context.JSON(http.StatusOK, resultDto)
}
