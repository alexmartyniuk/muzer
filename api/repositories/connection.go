package repositories

import (
	"github.com/jinzhu/gorm"
)

// CreateConnection ...
func CreateConnection() *gorm.DB {
	db, err := gorm.Open("postgres", "host=localhost user=postgres dbname=postgres sslmode=disable password=admin")
	if err != nil {
		panic(err)
	}

	return db
}
