package main

import (
	"bitbucket.org/Martinyuk/muzer/controllers"
	"bitbucket.org/Martinyuk/muzer/repositories"
)

func main() {
	repositories.MigrateScheme()

	router := controllers.NewRouter()
	router.Logger.Fatal(router.Start(":80"))
}
