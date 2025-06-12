# Umbraco-Docker-Workshop

This repository contains all the notes and worksheets for the Docker Workshop being given at Codegarden 2022.


The main workshop is in the [1-Workshop-Intro.md](/1-Workshop-Intro.md) file. Follow the instructions in there to get started.

*Please make sure you have gone through the prerequisites for this workshop in the [0-Prerequisites.md](/0-Prerequisites.md) file prior to starting.*

## Table of Contents

[0. Prerequisites](0-Prerequisites.md)
[1. Workshop Introduction](1-Workshop-Intro.md)
[2. Setting up a Database Container](2-Workshop-Database.md)
[3. Setting up a Website Container](3-Workshop-Website.md)



TODO: Volumes vs Bind Mounts
 - bind database with a bind mount to the folder inside th repo
 - Amend the sql script to attach the database from the bind mount?
    - Changes in the MDF/LDF file will show as repo changes - add a note about that
 - Create a path somehow (maybe a .env file) and use that in the docker-compose file to set the volume path
    - Add a note about the volume path being different on Windows and Linux
 - Is there a way to tell if your host machine is windows, mac or linux? 




# References and Resources
Slides - https://docs.google.com/presentation/d/1Nn1hfFkZp8QWpCsnIAGMH6IdBk0R551CiuU3IiJpBdk/

## Documentation for Docker

The following are suggested reading materials for Docker.

- Main Page : https://docs.docker.com/
- Networking : https://docs.docker.com/network/
- Docker Compose : https://docs.docker.com/compose/reference/
- Docker Hub : https://docs.docker.com/docker-hub/
- Storage : https://docs.docker.com/storage/
    - Volumes : https://docs.docker.com/storage/volumes/

## Umbraco

- Load Balancing
    - https://docs.umbraco.com/umbraco-cms/fundamentals/setup/server-setup/load-balancing/azure-web-apps
- Azure Blob Storage 
    - https://docs.umbraco.com/umbraco-cloud/set-up/media#about-azure-blob-storage
- Community Articles
    - https://swimburger.net/blog/umbraco/how-to-run-umbraco-9-as-a-linux-docker-container
    - https://codeshare.co.uk/blog/umbraco-9-useful-snippets/    
    - https://www.tutorialworks.com/container-networking/
    - https://github.com/dotnet/dotnet-docker/tree/main/samples/aspnetapp


## Notes

The following are notes for myself, but may be useful for others.
- HTTP Setting for openiddict
    - https://github.com/umbraco/Umbraco-CMS/pull/16614
- https://docs.umbraco.com/umbraco-cms/v/12.latest/reference/content-delivery-api#swagger
- https://emmti.com/headless-content-delivery-api-in-umbraco-12-rc-1#heading-installing-12rc1-on-your-local-machine
- SQL on M1
    - https://devblogs.microsoft.com/azure-sql/development-with-sql-in-containers-on-macos/
- SQL Edge Retirement : https://azure.microsoft.com/en-us/updates/v2/azure-sql-edge-retirement



global json force .net 8 for website
spaces!!