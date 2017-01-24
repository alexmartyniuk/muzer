package repositories

import (
	"github.com/jinzhu/gorm"
	_ "github.com/jinzhu/gorm/dialects/postgres"
)

type SourceType int

const (
	None        SourceType = 1
	Discogs     SourceType = 2
	ZaycevNet   SourceType = 3
	ProstoPleer SourceType = 4
)

type Artist struct {
	gorm.Model
	Source   SourceType
	SourceID int
	Name     string
	Thumb    string
	Albums   []Album
}

type Album struct {
	gorm.Model
	ArtistID uint
	Source   SourceType
	SourceID int
	Title    string
	Thumb    string
	Year     uint
	Tracks   []Track
}

type Track struct {
	gorm.Model
	AlbumID   uint
	Title     string
	Position  string
	Duration  uint
	Downloads []TrackDownload
}

type TrackDownload struct {
	gorm.Model
	TrackID   uint
	Source    SourceType
	SourceID  int
	URL       string
	Bitrate   uint
	Date      uint64
	Duration  uint
	TrackName string
	Size      uint
}

func (artist *Artist) Copy() *Artist {

	if artist == nil {
		return nil
	}

	return &Artist{
		artist.Model,
		artist.Source,
		artist.SourceID,
		artist.Name,
		artist.Thumb,
		artist.Albums,
	}
}

func (album *Album) Copy() *Album {

	if album == nil {
		return nil
	}

	return &Album{
		album.Model,
		album.ArtistID,
		album.Source,
		album.SourceID,
		album.Title,
		album.Thumb,
		album.Year,
		album.Tracks,
	}
}

func (track *Track) Copy() *Track {

	if track == nil {
		return nil
	}

	return &Track{
		track.Model,
		track.AlbumID,
		track.Title,
		track.Position,
		track.Duration,
		track.Downloads,
	}
}

// MigrateScheme creates database structure with all
// tables, indexes and initial data.
func MigrateScheme() {
	db := CreateConnection()
	defer db.Close()

	db = db.AutoMigrate(&Artist{}, &Album{}, &Track{}, &TrackDownload{})
	if db.Error != nil {
		panic(db.Error)
	}
}
