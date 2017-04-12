package services

import (
	"fmt"
	"net/url"
	"strconv"
	"sync"
	"github.com/jinzhu/gorm"
	zaycevnet   "github.com/AlexanderMartinyuk/go-zaycevnet/api"
	prostopleer "github.com/AlexanderMartinyuk/prostopleer"
	"bitbucket.org/Martinyuk/muzer/repositories"
)

// GetTrackWithDownloadByID ..
func GetTrackWithDownloadByID(db *gorm.DB, trackID uint) (*repositories.Track, *repositories.TrackDownload) {
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

	var wg sync.WaitGroup
	wg.Add(2)

	go func() {
        defer wg.Done()
        downloads := getTrackDownloadsFromZaycevnet(artistOriginal.Name, trackOriginal.Title)
		for _, download := range downloads {
			download.TrackID = trackOriginal.ID
			repositories.SaveTrackDownload(db, download)
		}
    }()

	go func() {
        defer wg.Done()
        downloads := getTrackDownloadsFromProstopleer(artistOriginal.Name, trackOriginal.Title)
		for _, download := range downloads {
			download.TrackID = trackOriginal.ID
			repositories.SaveTrackDownload(db, download)
		}
    }()
	
	wg.Wait()	

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

func getTrackDownloadsFromZaycevnet(artistName string, trackTitle string) (results []*repositories.TrackDownload) {
	client := zaycevnet.NewZClient(nil, "", "kmskoNkYHDnl3ol2")
	err := client.Auth()
	if err != nil {
		panic(err)
	}

	params := url.Values{}
	params.Add("query", fmt.Sprintf("%s %s", artistName, trackTitle))
	params.Add("page", strconv.Itoa(1))
	params.Add("type", "all")
	params.Add("sort", "")
	params.Add("style", "")

	result, err := client.Search(params)
	if err != nil {
		panic(err)
	}

	for _, track := range result.Tracks {
		downlaod, err := client.Download(track.ID)
		if err != nil {
			panic(err)
		}

		duration, _ := strconv.Atoi(track.Duration)
		trackDownload := repositories.TrackDownload{
			TrackID:   0,
			Source:    repositories.ZaycevNet,
			SourceID:  strconv.Itoa(track.ID),
			URL:       downlaod.URL,
			Bitrate:   uint(track.Bitrate),
			Duration:  uint(duration),
			TrackName: track.Track,
		}
		results = append(results, &trackDownload)
	}
	return results
}

func getTrackDownloadsFromProstopleer(artistName string, trackTitle string) (results []*repositories.TrackDownload) {
	api := &prostopleer.Api{
        User:     "129103",
        Password: "QF9SUdiR2NMR3EF4ySMW",
    }
	tracks, _, err := api.SearchTrack(fmt.Sprintf("artist:%s track:%s", artistName, trackTitle), "all", 1, 10)
	if err != nil {
		panic(err)
	}

    for _, track := range tracks {
        track.GetLink("listen")
        
		bitrate, _ := strconv.ParseUint(track.Bitrate, 10, 32)
		duration, _ := strconv.ParseUint(track.Duration, 10, 32)
		size, _ := strconv.ParseUint(track.Size, 10, 32)
		trackDownload := repositories.TrackDownload{
			TrackID:   0,
			Source:    repositories.ProstoPleer,
			SourceID:  track.Id,
			URL:       track.ListenUrl,
			Bitrate:   uint(bitrate),
			Duration:  uint(duration),
			TrackName: track.Name,
			Size:      uint(size),
		}

		results = append(results, &trackDownload)
    }
	return results
}