package services

import (
	"bitbucket.org/Martinyuk/discogs/api"
	"bitbucket.org/Martinyuk/muzer/api/repositories"
	"github.com/jinzhu/gorm"
	"strconv"
)

// SearchAndSaveArtistsByTerm ...
func SearchAndSaveArtistsByTerm(db *gorm.DB, searchTerm string) (artist []*repositories.Artist) {

	// 1. Create Discogs client and perform search for artists.
	discogsClient := api.NewDiscogsClient(nil, "HNmJpKxApdkwljeHZxXRMFGgGVMfsODoOJojIXfh")

	search, err := discogsClient.SearchArtist(searchTerm)
	if err != nil {
		panic(err)
	}

	// 2. Prepare array for results.
	results := make([]*repositories.Artist, len(search.Results))

	// 3. Save all found artists in DB and add into results list.
	for i, result := range search.Results {
		artist := repositories.Artist{
			Name:     result.Title,
			SourceID: result.ID,
			Thumb:    result.Thumb,
			Source:   repositories.Discogs,
		}

		results[i] = repositories.SaveArtistIfNotExists(db, &artist)
	}

	return results
}

// GetArtistWithAlbumsByID ..
func GetArtistWithAlbumsByID(db *gorm.DB, id uint) (artist *repositories.Artist) {

	// 1. Get artist by id.
	artist = repositories.GetArtistByID(db, id)
	if artist == nil {
		return nil
	}

	// 2. If artist has more than 1 album search was performed early
	// and we should just return read artist.
	if len(artist.Albums) > 0 {
		return artist
	}

	// 3. Create Discogs client and perform search for albums.
	discogsClient := api.NewDiscogsClient(nil, "HNmJpKxApdkwljeHZxXRMFGgGVMfsODoOJojIXfh")
	result, err := discogsClient.GetReleasesByArtistID(artist.SourceID)
	if err != nil {
		panic(err)
	}

	// 4. Save found results in DB.
	for _, release := range result.Releases {
		album := repositories.Album{
			ArtistID: artist.ID,
			Source:   artist.Source,
			SourceID: release.ID,
			Title:    release.Title,
			Thumb:    release.Thumb,
			Year:     uint(release.Year),
		}
		repositories.SaveAlbumIfNotExists(db, &album)
	}

	return repositories.GetArtistByID(db, id)
}

// GetArtistWithAlbumsByID ..
func GetAlbumWithTracksByID(db *gorm.DB, albumID uint) *repositories.Album {

	// 1. Get album by id.
	album := repositories.GetAlbumByID(db, albumID)
	if album == nil {
		return nil
	}

	// 2. If album has more than 1 track search was performed early
	// and we should just return read album.
	if len(album.Tracks) > 0 {
		return album
	}

	// 3. Create Discogs client and perform search for album details.
	discogsClient := api.NewDiscogsClient(nil, "HNmJpKxApdkwljeHZxXRMFGgGVMfsODoOJojIXfh")
	result, err := discogsClient.GetReleaseDetailsByID(album.SourceID)
	if err != nil {
		panic(err)
	}

	// 4. Save found results in DB.
	for _, trackItem := range result.Tracklist {
		track := repositories.Track{
			AlbumID:  album.ID,
			Title:    trackItem.Title,
			Position: trackItem.Position,
			Duration: 0,
		}

		duration, err := strconv.Atoi(trackItem.Duration)
		if err == nil {
			track.Duration = uint(duration)
		}

		repositories.SaveTrack(db, &track)
	}

	return repositories.GetAlbumByID(db, albumID)
}
