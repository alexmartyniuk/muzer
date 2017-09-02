# Muzer project #

The Muzer project created to allows people search and listen to free music from the Internet in one place.
The Muzer consists of two parts: server side application (API) written in Go and client side application (UI) written in JavaScript with ReactJS framework.

To build the whole application you first need install:

* [Go](https://golang.org/doc/install)
* [ReactJS](https://facebook.github.io/react/docs/installation.html)
* [PostgreSQL](https://www.postgresql.org/download/windows/)

To downlaod project files you need to run command:
```
go get bitbucket.org/Martinyuk/muzer
```
After that all projects files will be available in folder %USERPROFILE%\go\src\bitbucket.org\Martinyuk\muzer
You also need to dowload all required packages for api and ui projects:
```
cd  %USERPROFILE%\go\src\bitbucket.org\Martinyuk\muzer\api\
go get
```
```
cd  %USERPROFILE%\go\src\bitbucket.org\Martinyuk\muzer\ui\
npm install
```

To start API you need go to "api" folder and execute a command: 
```
go run main.go
```
API server started on 80 http port.

To start UI you need go to "ui" folder and execute a command:
```
npm start 
```
UI server started on 3000 http port.


To open the application you need to open a browser with http://localhost:3000/ address.