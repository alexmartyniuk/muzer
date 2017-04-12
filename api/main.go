package main

import (
	"bitbucket.org/Martinyuk/muzer/api/controllers"
	"bitbucket.org/Martinyuk/muzer/api/repositories"
)

func main() {
	repositories.MigrateScheme()

	router := controllers.NewRouter()
	router.Logger.Fatal(router.Start(":80"))
}
