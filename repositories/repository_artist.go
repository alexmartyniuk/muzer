package repositories

import (
	"github.com/jinzhu/gorm"
	_ "github.com/jinzhu/gorm/dialects/postgres"
)

// SaveArtistIfNotExists save the artist model in DB if artist with such Source and ExternalId is not exists.
// If artist with such Source and ExternaslId is exists original model is returned.
func SaveArtistIfNotExists(db *gorm.DB, artist *Artist) (result *Artist) {
	artistOriginal := getArtistByExternalID(db, artist.Source, artist.SourceID)

	if artistOriginal != nil {
		return artistOriginal
	}

	saveArtist(db, artist)

	return artist
}

// GetArtistByID get artist model by ID and return it.
func GetArtistByID(db *gorm.DB, artistID uint) (result *Artist) {
	artistOriginal := &Artist{}

	if db.Where(artistID).
		Preload("Albums").
		First(artistOriginal).
		RecordNotFound() {
		return nil
	}

	if db.Error != nil {
		panic(db.Error)
	}

	return artistOriginal
}

func saveArtist(db *gorm.DB, artist *Artist) (result *Artist) {
	db = db.Save(artist)

	if db.Error != nil {
		panic(db.Error)
	}

	return artist
}

func getArtistByExternalID(db *gorm.DB, externalSource SourceType, externalID int) (result *Artist) {
	artistOriginal := &Artist{}

	db = db.Where(Artist{SourceID: externalID, Source: externalSource}).First(artistOriginal)
	if db.RecordNotFound() {
		return nil
	}

	if db.Error != nil {
		panic(db.Error)
	}

	return artistOriginal
}
