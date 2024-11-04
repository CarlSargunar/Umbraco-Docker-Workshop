## Exercise 3. Running the Umbraco Site in a container

Now that the Umbraco site is working, we need to create a container for it. While running in the container, the Umbraco site will connect to the database in the umbData container directly. 

If the site is still running, stop it by running by pressing **Ctrl + c** in the terminal window. 

## 3.1 Create the Umbraco Site container

***Action:*** In the **UmbWeb** folder create a Dockerfile to define the components of the Umbraco container. Paste the contents below in that file, and make sure the line endings are **LF**. 

    # Use the SDK image to build and publish the website
    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /src
    COPY ["UmbWeb.csproj", "."]
    RUN dotnet restore "UmbWeb.csproj"
    COPY . .
    RUN dotnet publish "UmbWeb.csproj" -c Release -o /app/publish

    # Copy the published output to the final running image
    FROM mcr.microsoft.com/dotnet/aspnet:780 AS final 
    WORKDIR /app

    # Copy the published output to the final running image
    COPY --from=build /app/publish .

    # Copy the media items to the final running image
    COPY ./wwwroot/media ./wwwroot/media

    # Set the entrypoint to the web application
    ENTRYPOINT ["dotnet", "UmbWeb.dll"]


This Dockerfile starts with a build image which contains the SDK to actually compile the project, and one with ASP.NET runtimes to actually host the running application. The running application doesn't need any build tools, so we don't include them. From the above Dockerfile we can see the stages of the build process.

1. Starting on the main image, we will use the SDK image to compile the project.
2. Copying the working project folder to the build image
3. Run the restore command to download the dependencies
4. Compile and publish the output of the project
5. Switch to the hosting image 
6. Copy the published output to the final image
7. Copy the media files to the final image
8. Set the entrypoint to the binary output of the main project

## 3.2 Building the Umbraco Site image, setting a network and running it

Once the Dockerfile exists, we need to create a configuration which lets the website contianer connect to the database container. 

***Action:*** Create a copy of the **appsettings.Development.json** called **appsettings.Staging.json**. In that file ensure the connectionstring is set-up to connect to **umbdata** as the database server. You will need to add the following connectionstring section to the file as a sibling of the Umbraco node. 

    "ConnectionStrings": {
        "umbracoDbDSN": "Server=umbdata;Database=UmbracoDb;User Id=sa;Password=P@55word!!;TrustServerCertificate=true",     "umbracoDbDSN_ProviderName": "Microsoft.Data.SqlClient"
    }


You could altenatively copy the already edited from from the **/Files/UmbWeb/appsettings.Staging.json** file in the repository.

Finally we can compile a docker image for the Umbraco site. 

***Action:*** Run the following command to build the image.

    docker build --tag=umbweb ./UmbWeb

This will download the required components and compile a final image ready to run the site in a container, and may take some time. 

At this point we can see all the images we have created by using the following command

    docker images

## 3.3 Running the website container in the same network

We can then run the website container. *Notice in the command below there is an argument to let the container know which network to connect to - the same **umbNet** network*. Here we are doing this using the **docker run --network** flag instead of using an explicit command.

***Action:*** Run the following command to run the website container.

    docker run --name umbweb -p 8000:80 -v umb_media:/app/wwwroot/media -v umb_logs:/app/umbraco/Logs -e ASPNETCORE_ENVIRONMENT='Staging' --network=umbNet -d umbweb

In the above command you can also see the volumes we use with the application container - specifically the log and the media folders. The reason to use these is that with media we want to share the media library if we should want to create more running sites (as we will later in the course) and with logs, we want to be able to view these logs and diagnose issues if the container isn't able to run for any reason.

Similarly with media, we want all website containers to be able to share the same media library, so we can use a volume for this so the images are stored on the docker host and not in the container.

One other thing we can see is the Environment variable we are passing the container with the -e flag, which sets our AspNetCore Environment to staging, and thus causes the container to run with the appsettings.staging.json file and allow us to connect to the database.

You can now see the website running by visiting:
    
    http://localhost:8000

Once the container is running, if you run a docker ps command, you'll see both the database and website containers running.

    docker ps

![Running Containers](media/3_DockerPS.png)

You can also see the status of running containers and logs by running the Docker Desktop application.

## Additional Reading - Networks and Volumes

Docker networks and volumes won't be covered in depth during this workshop as it's time-limited, but if you wish to read further, the following links may be useful.

- [Docker Networks](https://docs.docker.com/network/)
- [Docker Volumes](https://docs.docker.com/storage/volumes/)


