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

For all instructions, it is assumed you will be working in the root folder of this project. It's recommended the first step is to fork this repository so you can have your own copy of it and then clone it onto your machine. If you don't have a github account, you can download a zip of this repository and extract it to your machine.

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

# 2. Create the Database Container

Slides - Before we start the next stages we will look at the following concepts

- What is a container?
    - Virtual machine vs Docker container
    - Components of a container
    - Docker container vs. Docker image
    - Describe Dockerfile

### 2.1 Create a container for the database server

Here we will create a container for the database and run the site against this database. Create a folder called UmbDock in the root folder of this project. 

Create a blank file in the UmbDock folder called Dockerfile. This will define the database container, and also describe the configuration we will use with that database container. 

*Note : the case of the file is important - it needs to be called Dockerfile with no extension*

In that file we will define the image we will use, and the ports we will use.

    FROM mcr.microsoft.com/azure-sql-edge:1.0.4

    ENV ACCEPT_EULA=Y
    ENV SA_PASSWORD=SQL_password123

    USER root
    
    RUN mkdir /var/opt/sqlserver
    
    RUN chown mssql /var/opt/sqlserver
    
    ENV MSSQL_BACKUP_DIR="/var/opt/sqlserver"
    ENV MSSQL_DATA_DIR="/var/opt/sqlserver"
    ENV MSSQL_LOG_DIR="/var/opt/sqlserver"

    EXPOSE 1433/tcp

    COPY setup.sql /
    COPY startup.sh /

    COPY Umbraco.mdf /var/opt/sqlserver
    COPY Umbraco_log.ldf /var/opt/sqlserver

    ENTRYPOINT [ "/bin/bash", "startup.sh" ]
    CMD [ "/opt/mssql/bin/sqlservr" ]    

*Note : We are use Azure SQL Edge here as a database container in case there is anyone using a Macbook with an M1 chip as these run on the Arm architecture.*

Once the Dockerfiles is created, we also need to copy the Umbraco database to the UmbData folder. There are 2 files that need to be copied into the root of the UmbData folder.

- /UmbDock/umbraco/Data/Umbraco.mdf
- /UmbDock/umbraco/Data/Umbraco_log.ldf

There are 2 other files created in this repository which we need to copy into the UmbData folder.

- /Files/UmbData/setup.sql
- /Files/UmbData/startup.sh

Finally edit the appsettings.Development.json file to set the connection string to the database.

    "ConnectionStrings": {
        "umbracoDbDSN": "Server=localhost;Database=UmbracoDb;User Id=sa;Password=SQL_password123;", "umbracoDbDSN_ProviderName": "Microsoft.Data.SqlClient"
    },    

*Note : If you are running a local SQL Server on your machine, you will need to stop that server before you can run the database container.*

## 2.2 Build the database image and run the database container

Before you run the database container, make sure the rest of the files have the the right file endings. These files all need to have the Linux line ending (\n) and not the Windows line ending (\r\n). 

Once this is done, build the database image.

    docker build --tag=umbdata .\UmbData    

And run it

    docker run --name umbdata -p 1433:1433 --volume sqlserver:/var/opt/sqlserver -d umbdata

This should give you a container ID. You can check which containers are running by running the following command.

    docker ps


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



