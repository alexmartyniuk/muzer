package repositories

import (
	"github.com/jinzhu/gorm"
	_ "github.com/jinzhu/gorm/dialects/postgres"
)

func saveAlbum(db *gorm.DB, album *Album) (result *Album) {
	db = db.Save(album)

	if db.Error != nil {
		panic(db.Error)
	}

	return album
}

func getAlbumByExternalID(db *gorm.DB, externalSource SourceType, externalID int) (result *Album) {
	albumOriginal := &Album{}

	db = db.Where(Album{SourceID: externalID, Source: externalSource}).First(albumOriginal)
	if db.RecordNotFound() {
		return nil
	}

	if db.Error != nil {
		panic(db.Error)
	}

	return albumOriginal
}

// SaveAlbum save the album model and return saved object with ID.
func SaveAlbumIfNotExists(db *gorm.DB, album *Album) (result *Album) {
	var albumOriginal = getAlbumByExternalID(db, album.Source, album.SourceID)

	if albumOriginal != nil {
		return albumOriginal
	}

	saveAlbum(db, album)

	return album
}

// GetAlbumByID get album model with assosiated tracks by ID and return it.
func GetAlbumByID(db *gorm.DB, albumID uint) (result *Album) {
	var albumOriginal = &Album{}

	if db.Where(albumID).
		Preload("Tracks").
		First(albumOriginal).
		RecordNotFound() {
		return nil
	}

	if db.Error != nil {
		panic(db.Error)
	}

	return albumOriginal
}
