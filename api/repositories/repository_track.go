package repositories

import (
	"fmt"
	"github.com/jinzhu/gorm"
	_ "github.com/jinzhu/gorm/dialects/postgres"
)

// SaveTrack save the track model in DB and return saved object with ID.
func SaveTrack(db *gorm.DB, track *Track) (result *Track) {

	db = db.Save(track)

	if db.Error != nil {
		panic(db.Error)
	}

	return track
}

// SaveTrackDownload save the track download model in DB and return saved object with ID.
func SaveTrackDownload(db *gorm.DB, trackDownload *TrackDownload) (result *TrackDownload) {

	db = db.Save(trackDownload)

	if db.Error != nil {
		panic(db.Error)
	}

	return trackDownload
}

// GetTrackByID ...
func GetTrackByID(db *gorm.DB, trackID uint) (result *Track) {
	return getTrackByID(db, trackID, false)
}

// GetTrackWithDownloadsByID ...
func GetTrackWithDownloadsByID(db *gorm.DB, trackID uint) (result *Track) {
	return getTrackByID(db, trackID, true)
}

// GetArtistByTrackID ...
func GetArtistByTrackID(db *gorm.DB, trackID uint) (result *Artist) {

	trackOriginal := GetTrackByID(db, trackID)
	if trackOriginal == nil {
		return nil
	}

	albumOriginal := GetAlbumByID(db, trackOriginal.AlbumID)
	if albumOriginal == nil {
		panic(fmt.Sprintf("Cannot get album by track_id='%d'", trackID))
	}

	artistOriginal := GetArtistByID(db, albumOriginal.ArtistID)
	if artistOriginal == nil {
		panic(fmt.Sprintf("Cannot get artist by track_id='%d'", trackID))
	}

	return artistOriginal
}

func getTrackByID(db *gorm.DB, trackID uint, fetchDownloads bool) (result *Track) {
	trackOriginal := &Track{}

	db = db.Where(trackID)
	if fetchDownloads {
		db = db.Preload("Downloads")
	}
	db = db.First(trackOriginal)

	if db.RecordNotFound() {
		return nil
	}

	if db.Error != nil {
		panic(db.Error)
	}

	return trackOriginal
}

func getTrackDownloadByExternalID(db *gorm.DB, externalSource SourceType, externalID string) (result *TrackDownload) {
	downloadOriginal := &TrackDownload{}

	db = db.Where(TrackDownload{Source: externalSource, SourceID: externalID}).First(downloadOriginal)
	if db.RecordNotFound() {
		return nil
	}

	if db.Error != nil {
		panic(db.Error)
	}

	return downloadOriginal
}
