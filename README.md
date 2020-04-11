# Gammification with MS Learn

## Netcoreconf 2020 (Virtual)

Repo for the "Gammification with MS Learn" project shown on @netcoreconf 2020 (virtual flavor).

Microsoft Learn website: <https://docs.microsoft.com/en-us/learn>  
Recorded session on Youtube: <https://youtu.be/-9PKSEGKkbE>

## CosmosDb

Optional CosmosDb initial setup included.  
Run **DB_SETUP\CREATE_DATABASE.sh** from Azure Cloud Shell.

## API

API project based on Azure Functions.  
You can publish to Azure or run it locally.

Example calls:  

### Create user

    POST http://localhost:7071/api/CreateUser
    Content-Type: application/json

    {
      "username": "User",
      "level": "LEVEL X",
      "points": "1000",
      "levelStatusPoints": "1000/0000 XP"
    }

### Update user

    PUT http://localhost:7071/api/UpdateUser
    Content-Type: application/json

    {
      "isSuccess": true,
      "errorMessage": null,
      "username": "User",
      "level": "LEVEL X",
      "points": "1300",
      "levelStatusPoints": "1300/0000 XP"
    }

### Get users

    GET http://localhost:7071/api/GetUsers
    Content-Type: application/json

## Frontend

Basic Vue.js frontend to show the rank. To initialize frontend application just run:

     npm start

## Scraper

API containerized in a Docker image to get MS Learn website information.  
Docker instructions:

     docker build --no-cache --rm -f "Dockerfile" -t mslearnscraper:latest "."
     docker run --rm -it  -p 8081:443/tcp -p 8080:80/tcp mslearnscraper:latest
