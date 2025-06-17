# Umbraco-Docker-Workshop

This repository contains all the notes and worksheets for the Docker Workshop being given at Umbraco Codegarden. It will be based on the latest LTS version of Umbraco, which is currently Umbraco 13. 
 
*Please make sure you have gone through the prerequisites for this workshop in the [0-Prerequisites.md](/0-Prerequisites.md) file prior to starting.*

## Table of Contents

0. [Prerequisites](0-Prerequisites.md)
1. [Workshop Introduction](1-Workshop-Intro.md)
2. [Simple Umbraco Container](2-Simple-Umbraco-Container.md)
3. [Database Container](3-Database-Container.md)
4. [Umbraco Website](4-Umbraco-Webite.md)
5. [Website Container](5-Website-Container.md)
6. [Content Delivery Api](6-Content-Delivery-Api.md)
7. [Blazor SPA](7-Blazor-SPA.md)
8. [Docker Compose](8-Docker-Compose.md)
9. [Conclusion](9-Conclusion.md)
10. [Docker CLI Quick Start](10-Docker-CLI-Quick-Start.md)

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