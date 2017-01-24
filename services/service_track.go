package services

import (
	"bitbucket.org/Martinyuk/muzer/repositories"
	"fmt"
	"github.com/jinzhu/gorm"
	"github.com/pixfid/go-zaycevnet/api"
	"net/url"
	"strconv"
)

// GetTrackWithDownloadByID ..
func GetTrackWithDownloadByID(db *gorm.DB, trackID uint) (*repositories.Track, *repositories.TrackDownload) {

	// 1. Get track by id.
	trackOriginal := repositories.GetTrackWithDownloadsByID(db, trackID)
	if trackOriginal == nil {
		return nil, nil
	}

	trackDownload := getMostAppropriateTrackDownload(trackOriginal)
	if trackDownload != nil {
		return trackOriginal, trackDownload
	}

	artistOriginal := repositories.GetArtistByTrackID(db, trackOriginal.ID)
	if artistOriginal == nil {
		panic(fmt.Sprintf("Cannot get artist by track_id=%d", trackID))
	}

	// 3. Create ZacevNet client and perform search for track downloads.
	client := api.NewZClient(nil, "", "kmskoNkYHDnl3ol2")
	err := client.Auth()
	params := url.Values{}
	params.Add("query", fmt.Sprintf("%s %s", artistOriginal.Name, trackOriginal.Title))
	params.Add("page", strconv.Itoa(1))
	params.Add("type", "all")
	params.Add("sort", "")
	params.Add("style", "")

	result, err := client.Search(params)
	if err != nil {
		panic(err)
	}

	// 4. Save found results in DB.
	for _, track := range result.Tracks {
		println(track.Track, "  ", track.Bitrate)

		downlaod, err := client.Download(track.ID)
		if err != nil {
			panic(err)
		}

		trackDownload := repositories.TrackDownload{
			TrackID:   trackOriginal.ID,
			Source:    repositories.ZaycevNet,
			SourceID:  track.ID,
			URL:       downlaod.URL,
			Bitrate:   uint(track.Bitrate),
			Date:      uint64(track.Date),
			Duration:  0,
			TrackName: track.Track,
		}

		duration, err := strconv.Atoi(track.Duration)
		if err == nil {
			trackDownload.Duration = uint(duration)
		}

		repositories.SaveTrackDownload(db, &trackDownload)

	}

	trackOriginal = repositories.GetTrackWithDownloadsByID(db, trackID)
	if trackOriginal == nil {
		return nil, nil
	}

	trackDownload = getMostAppropriateTrackDownload(trackOriginal)
	if trackDownload == nil {
		return trackOriginal, nil
	}

	return trackOriginal, trackDownload
}

func getMostAppropriateTrackDownload(track *repositories.Track) *repositories.TrackDownload {
	if len(track.Downloads) == 0 {
		return nil
	}

	return &track.Downloads[0]
}
