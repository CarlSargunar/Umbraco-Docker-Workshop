# Umbraco and Docker

## Prerequisites

In order to run this application you will need the following installed on your machine.

- Visual Studio Code
    - There's a useful docker extension for Visual studio code : [https://code.visualstudio.com/docs/containers/overview](https://code.visualstudio.com/docs/containers/overview)
- Docker Desktop 
    - Windows subsystem for Linux (only required in Windows obviously)	
- .NET SDK version 6
    - https://dotnet.microsoft.com/en-us/download/dotnet/6.0

For the full list of prerequisites, please see the [prerequisites](Prerequisites.md) document.

# 1. Creating the a basic Umbraco Site

## Installing Umbraco Template and start Website

Run the following to install the umbraco template.

    dotnet new -i Umbraco.Templates::10.0.0-rc4

Set the SDK Version being used and Create solution/project. This will create a global file with the current latest version of the SDK, and a blank solution which you can use with Visual Studio if you prefer to use that.

    dotnet new globaljson --sdk-version 6.0 --force 
    dotnet new sln --name UmbDock

## 1.1 Start a new blank Umbraco Site

Create a new Umbraco site using the following command. This will define the name of the site and the default database, as well as the default admin user and password. Here we will be using SQL LocalDB as the database so that in later steps it can be imported directly into the production database server. 

    dotnet new umbraco -n UmbDock --friendly-name "Admin User" --email "admin@admin.com" --password "1234567890" --connection-string "Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Umbraco.mdf;Integrated Security=True"

Add project to the solution.

    dotnet sln add UmbDock

## 1.2 Install a template site for the exercise. 

This workshop will be using the Clean starter kit for Umbraco. This is a great starting point, and will let us focus on the docker integration while giving us a great site to work with. 

    dotnet add UmbDock package Clean

At this point you can choose to continue in either Visual Studio or VS Code. 

Run the website by issueing the following command.

    dotnet run --project UmbDock

This should, if there are no errors, start up the kestrel server and serve the site for you to browse.

![2_run_site](media/2_run_site.png)

If you browse the site at https://localhost:11608 (or whatever port your computer reports) you should be able to see the site running.

## 2. Create the Database Container

## Create Network

Create the bridge network 

    docker network create -d bridge umbNet    

## Build the docker image

First copy the Docker files.

For the website

    docker build --tag=umbdock .\UmbDock

For the database

    docker build --tag=umbdata .\UmbData    

## Run the containers

Website

    docker run --name umbdock -p 8000:80 -v media:/app/wwwroot/media -v logs:/app/umbraco/Logs -e ASPNETCORE_ENVIRONMENT='Staging' --network=umbNet -d umbdock

    docker run --name umbdock2 -p 8002:80 -v media:/app/wwwroot/media -v logs:/app/umbraco/Logs -e ASPNETCORE_ENVIRONMENT='Staging' --network=umbNet -d umbdock

Data

    docker run --name umbdata -p 1433:1433 --volume sqlserver:/var/opt/sqlserver --network=umbNet -d umbdata

# Add the Blazor Container

## Build Image

    docker build --tag=umblazor .\UmBlazor    

## Run the Container

    docker run --name umblazor -p 8001:80 -e ASPNETCORE_ENVIRONMENT='Staging' --network=umbNet -d umblazor

## File Types

Remember, Linux line endings need -> Lf NOT CrLf

## Cleanup

If you want to remove the RC1 template and revert to the older version

    dotnet new -u Umbraco.Templates

## Troubleshooting 

Trust the Dev Certs

    dotnet dev-certs https --trust

Clear your local nuget cache

    dotnet nuget locals all --clear



