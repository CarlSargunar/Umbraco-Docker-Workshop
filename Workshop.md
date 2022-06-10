# Umbraco and Docker

## Prerequisites

In order to participate in this workshop you will need to ensure you have the full list of prerequisites, please see the [prerequisites](Prerequisites.md) document for details.


### Starting on a non-windows dev box

TODO : Mac instructions

If you are doing this workshop on a non-windows box, the order in which we need to go through is slightly differert, since the SQL LocalDB database isn't supported outside windows. Please raise your hand and get in touch with me if that is the case, and I can go through what you need.


# 1. Creating the a basic Umbraco Site

For all instructions, it is assumed you will be working in the root folder of this project. It's recommended the first step is to fork this repository so you can have your own copy of it and then clone it onto your machine. If you don't have a github account, you can download a zip of this repository and extract it to your machine.

## Installing Umbraco Template and start Website

Run the following to install the umbraco template.

    dotnet new -i Umbraco.Templates::10.0.0-rc4

Set the SDK Version being used and Create solution/project. This will create a global file with the current latest version of the SDK, and a blank solution which you can use with Visual Studio if you prefer to use that.

    dotnet new globaljson --sdk-version 6.0 --force 

## 1.1 Start a new blank Umbraco Site

Create a new Umbraco site using the following command. This will define the name of the site and the default database, as well as the default admin user and password. Here we will be using SQL LocalDB as the database so that in later steps it can be imported directly into the production database server. 

    dotnet new umbraco -n UmbDock --friendly-name "Admin User" --email "admin@admin.com" --password "1234567890" --connection-string "Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Umbraco.mdf;Integrated Security=True" --development-database-type LocalDB

There is a great tool to help you configure the the unattended installation options for umbraco at [https://psw.codeshare.co.uk/](https://psw.codeshare.co.uk/)

## 1.2 Install a template site for the exercise. 

This workshop will be using the Clean starter kit for Umbraco. This is a great starting point, and will let us focus on the docker integration while giving us a great site to work with. 

    dotnet add UmbDock package Clean

At this point you can choose to continue in either Visual Studio or VS Code. 

Run the website by issuing the following command.

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

Finally,in the UmbDock projet edit the appsettings.Development.json file so that the Umbraco site can connet to the database in the container rather than the local file version.

    "ConnectionStrings": {
        "umbracoDbDSN": "Server=localhost;Database=UmbracoDb;User Id=sa;Password=SQL_password123;", "umbracoDbDSN_ProviderName": "Microsoft.Data.SqlClient"
    }    

Todo : correct DNS


## 2.2 Build the database image and run the database container

Before you run the database container, make sure the rest of the files have the the right file endings. These files all need to have the Linux line ending (\n) and not the Windows line ending (\r\n). 

*Note : If you are running a local SQL Server on your machine, you will need to stop that server before you can run the database container.*

Once this is done, build the database image.

    docker build --tag=umbdata .\UmbData    

And run it

    docker run --name umbdata -p 1433:1433 --volume sqlserver:/var/opt/sqlserver -d umbdata

This should give you a container ID. You can check which containers are running by running the following command.

    docker ps


## 3 Running the Umbraco Site in a container

Now that the Umbraco site is running through Kestrel but conneting to the database server in the container, we need to create a container for the Umbraco site. 

## 3.1 Create the Umbraco Site container

In the Umbraco UmbDock project we will be creating a Dockerfile to define how it will be hosted. 

    # Use the SDK image to build and publish the website
    FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
    WORKDIR /src
    COPY ["UmbDock.csproj", "."]
    RUN dotnet restore "UmbDock.csproj"
    COPY . .
    RUN dotnet publish "UmbDock.csproj" -c Release -o /app/publish

    # Copy the published output to the final running image
    FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final 
    WORKDIR /app
    COPY --from=build /app/publish .
    ENTRYPOINT ["dotnet", "UmbDock.dll"]

This Dockerfile starts with a build image which contains the SDK to actually compile the project, and one with ASP.NET runtimes to actuall host the running application. From the above Dockerfile we can see the stages of the build process.

- Starting on the main image, we will use the SDK image to compile the project.
- Copy the working project folder to the build image
- Run the restore command to download the dependencies
- Compile and publish the output of the project
- Switch to the hosting image and copy the published output to the final image
- Set the entrypoint to the binary output of the main project

## 3.2 Building the Umbraco Site image, setting a network and running it

Once the Dockerfile exists, we need to create a configuration which lets the website contianer connect to the database container. Create a copy of the appsettings.Development.json called appsettings.Staging.json, and in that file ensure the connectionstring is set-up to connect to umbdata as a container name.

    "ConnectionStrings": {
        "umbracoDbDSN": "Server=umbdata;Database=UmbracoDb;User Id=sa;Password=SQL_password123;",     "umbracoDbDSN_ProviderName": "Microsoft.Data.SqlClient"
    }

Finally we can compile a docker image for the Umbraco site.

    docker build --tag=umbdock .\UmbDock

This will download the required components and compile a final image ready to run the site in a container, and may take some time. However before we are able to run both the site and the database container, we need to set up the network. 

## 4 Docker Networks and Volumes

Slides - Before we start the next stages we will look at the following concepts

    - Container Networking
        - Bridge Network
        - Custom bridge Network
        - Host Network
        - Others
            - Overlay
            - Macvlan
            - Ipvlan
    - Container volumes
        - Volumes
        - Bind mounts
        - tmpfs mounts

## 4.1 Creating the network for our containers

To let the website and database containers communicate with each other, we need to define a custom bridge network between the two of them. 

    docker network create -d bridge umbNet    

We then need to run the database and website containers attached to this network. Since the database container is already running, we can issue the following command to attach the container to the network.

    docker network connect umbNet umbdata

We can then run the website container. Notice in the command below there is an argument to let the container know which network to connect to.

    docker run --name umbdock -p 8000:80 -v media:/app/wwwroot/media -v logs:/app/umbraco/Logs -e ASPNETCORE_ENVIRONMENT='Staging' --network=umbNet -d umbdock

In the above command you can also see the volumes we use with the application container - specifically the log and the media folders. The reason to use these is that with media we want to share the media library if we should want to create more running sites (as we will later in the course) and with logs, we want to be able to view these logs and diagnose issues if the container isn't able to run for any reason.

One other thing we can see is the Environment variable we are passing the container with the -e flag, which sets our AspNetCore Environment to staging, and thus causes the container to run with the appsettings.staging.json file and allow us to connect to the database.

Once the container is running, if you run a docker ps command, you'll see both the database and website containers running.




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

## Docker compose

docker compose build
docker compose up
docker compose down

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



