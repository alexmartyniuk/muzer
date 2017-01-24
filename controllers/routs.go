package controllers

import (
	"bitbucket.org/Martinyuk/muzer/repositories"
	"fmt"
	"github.com/jinzhu/gorm"
	"github.com/labstack/echo"
	"runtime"
)

type Route struct {
	Method      string
	Pattern     string
	HandlerFunc echo.HandlerFunc
}

type Routes []Route

type Context struct {
	echo.Context
	Transaction *gorm.DB
}

// NewRouter creates new router
func NewRouter() (e *echo.Echo) {

	router := echo.New()

	router.Use(transaction)
	//router.Use(middleware.Logger())

	for _, route := range routes {

		switch route.Method {
		case "GET":
			router.GET(route.Pattern, route.HandlerFunc)
		case "POST":
			router.POST(route.Pattern, route.HandlerFunc)
		case "PUT":
			router.PUT(route.Pattern, route.HandlerFunc)
		case "DELETE":
			router.DELETE(route.Pattern, route.HandlerFunc)
		default:
		}
	}

	return router
}

var routes = Routes{
	Route{
		"GET",
		"/artists/find", // ?query=Amy Winehouse
		FindArtists,
	},
	Route{
		"GET",
		"/artists/:artist_id",
		GetArtistDetails,
	},
	Route{
		"GET",
		"/albums/:album_id",
		GetAlbumDetails,
	},
	Route{
		"GET",
		"/tracks/:track_id",
		GetTrackDetails,
	},
}

func transaction(h echo.HandlerFunc) echo.HandlerFunc {
	return func(c echo.Context) error {

		db := repositories.CreateConnection()
		transaction := db.Begin()
		cc := &Context{c, transaction}

		defer func() {
			if r := recover(); r != nil {
				var err error
				switch r := r.(type) {
				case error:
					err = r
				default:
					err = fmt.Errorf("%v", r)
				}
				stack := make([]byte, 1024)
				length := runtime.Stack(stack, true)

				cc.Transaction.Rollback()

				cc.Logger().Printf("[%s] %s %s\n", "PANIC RECOVER", err, stack[:length])
				cc.Error(err)
			} else {
				cc.Transaction.Commit()
			}

			cc.Transaction.Close()
		}()

		return h(cc)
	}
}
