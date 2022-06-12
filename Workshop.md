# Umbraco and Docker

## Prerequisites

In order to participate in this workshop you will need to ensure you have the full list of prerequisites, please see the [prerequisites](Prerequisites.md) document for details.


# Slides : What is a container?

Before we start the next stages we will look at the following concepts. The link to the slides used throughout this presentation is https://docs.google.com/presentation/d/1MYf3CkzKYx-vZS0ntKIbNPejYVDbU2iPQit1OTzsoUM/


- What is a container?
    - Virtual machine vs Docker container
    - Components of a container
    - Docker container vs. Docker image
    - Describe Dockerfile

# Exercise 1 - Create a Database Container

For all instructions, it is assumed you will be working in the root folder of this project. There is a folder called 'Files' which contains the files used in this workshop to save you typing them out manually.

It's recommended the first step is to fork this repository on Github so you can have your own copy of it and then clone it onto your machine. If you don't have a github account, you can download a zip of this repository and extract it to your machine.

## 1.1 Create a container for the database server

Here we will create a container for the database and run the site against this database. Create a folder called UmbData in the root folder of this project. 

Create a blank file in the UmbData folder called Dockerfile. This will define the database container, and also describe the configuration we will use with that database container. 

*Note : the case of the file is important - it needs to be called Dockerfile with no extension*

In that file we will define the image we will use, and the ports we will use.

    FROM mcr.microsoft.com/azure-sql-edge:1.0.4

    ENV ACCEPT_EULA=Y
    ENV SA_PASSWORD=SQL_password123

    USER root
    
    ENV MSSQL_BACKUP_DIR="/var/opt/mssql"
    ENV MSSQL_DATA_DIR="/var/opt/mssql/data"
    ENV MSSQL_LOG_DIR="/var/opt/mssql/log"

    EXPOSE 1433/tcp

    COPY setup.sql /
    COPY startup.sh /

    ENTRYPOINT [ "/bin/bash", "startup.sh" ]
    CMD [ "/opt/mssql/bin/sqlservr" ]   

*Note : We are use Azure SQL Edge here as a database container in case there is anyone using a Macbook with an M1 chip as these run on the Arm architecture.*

There are 2 other files created in this repository which we need to copy into the UmbData folder.

- /Files/UmbData/setup.sql
- /Files/UmbData/startup.sh

These two files will be used to create a blank database if none exists when the database container starts. That way when the website starts it will already have a blank database ready to use.

## 1.2 Windows vs Linux Line Endings

Historically windows terminates line-endings in file with a carriage return and line feed (CRLF), while Linux uses a single line feed (LF) - and if you want to learn about the history of why then check out this awesome video from Scott Hanselman : [https://www.youtube.com/watch?v=TtiBhktB4Qg](https://www.youtube.com/watch?v=TtiBhktB4Qg)

To that end, we need to make sure all our files related to this and any containers are terminated with Line Feed (LF) and NOT Carriage Return Line Feed (CRLF).

In VS Code, this can be done using the option as shown below.

![image](media/6_VSCodeLineEndings.png)

## 1.3 Build the database image and run the database container

Before you run the database container, make sure the rest of the files have the the right file endings. These files all need to have the Linux line ending (\n) and not the Windows line ending (\r\n). 


Once this is done, build the database image.

    docker build --tag=umbdata ./UmbData    

And run it with the following command. 

*Note : If you are running a local SQL Server on your machine, you will need to stop that server before you can run the database container, or the container will not be able to start.*


    docker run --name umbdata -p 1433:1433 --volume sqlFiles:/var/opt/mssql  -d umbdata

This should give you a container ID back if the container was started successfully. 

## 1.4 Creating the network for our containers

To let the website and database containers communicate with each other, we need to define a custom bridge network between the two of them. 

    docker network create -d bridge umbNet    

We then need to run the database and website containers attached to this network. Since the database container is already running, we can issue the following command to attach the container to the network.

    docker network connect umbNet umbdata

You can inspect the network by running the following command.

    docker network inspect umbNet

# 2. Creating the a basic Umbraco Site

Now that we have a database container running, we are going to create our Umbraco website. We will create it first as as a normal website running on the file system, and not in a container. 

## Installing Umbraco Template and start Website

Run the following to install the umbraco template.

    dotnet new -i Umbraco.Templates::10.0.0-rc5

Set the SDK Version being used and Create solution/project. This will create a global file with the current latest version of the SDK, and a blank solution which you can use with Visual Studio if you prefer to use that.

    dotnet new globaljson --sdk-version 6.0 --force 

## 2.1 Start a new blank Umbraco Site

Create a new Umbraco site using the following command. This will define the name of the site and the default database, as well as the default admin user and password. Here we will be using SQL LocalDB as the database so that in later steps it can be imported directly into the production database server. 

    dotnet new umbraco -n UmbWeb --friendly-name "Admin User" --email "admin@admin.com" --password "1234567890" --connection-string "Server=localhost;Database=UmbracoDb;User Id=sa;Password=SQL_password123;" 

## 2.2 Install a template site for the exercise. 

This workshop will use the Clean starter kit for Umbraco. This is a great starting point, and will let us focus on the docker integration while giving us a great site to work with. 

    dotnet add UmbWeb package Clean

Run the website by issueing the following command.

    dotnet run --project UmbWeb

This should, if there are no errors, start up the kestrel server and serve the site for you to browse.

![2_run_site](media/2_run_site.png)

If you browse the site at https://localhost:11608 (or whatever port your computer reports) you should be able to see the site running.

## 3 Running the Umbraco Site in a container

Now that the Umbraco site is running through Kestrel but conneting to the database server in the container, we need to create a container for the Umbraco site. 

If the site is still running, stop it by running by pressing Ctrl + c in the terminal window. 

## 3.1 Create the Umbraco Site container

In the Umbraco UmbWeb project we will be creating a Dockerfile to define how it will be hosted. 

    # Use the SDK image to build and publish the website
    FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
    WORKDIR /src
    COPY ["UmbWeb.csproj", "."]
    RUN dotnet restore "UmbWeb.csproj"
    COPY . .
    RUN dotnet publish "UmbWeb.csproj" -c Release -o /app/publish

    # Copy the published output to the final running image
    FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final 
    WORKDIR /app
    COPY --from=build /app/publish .
    ENTRYPOINT ["dotnet", "UmbWeb.dll"]

This Dockerfile starts with a build image which contains the SDK to actually compile the project, and one with ASP.NET runtimes to actuall host the running application. From the above Dockerfile we can see the stages of the build process.

1. Starting on the main image, we will use the SDK image to compile the project.
2. Copy the working project folder to the build image
3. Run the restore command to download the dependencies
4. Compile and publish the output of the project
5. Switch to the hosting image and copy the published output to the final image
6. Set the entrypoint to the binary output of the main project

## 3.2 Modify the project to include Media

In order to include the media files which came with the template, in VS Code you need to add the following to the UmbWeb.csproj project file.

    <ItemGroup>
        <Content Include="wwwroot\media\**" />
    </ItemGroup>


## 3.3 Building the Umbraco Site image, setting a network and running it

Once the Dockerfile exists, we need to create a configuration which lets the website contianer connect to the database container. Create a copy of the appsettings.Development.json called appsettings.Staging.json, and in that file ensure the connectionstring is set-up to connect to umbdata as a container name.

    "ConnectionStrings": {
        "umbracoDbDSN": "Server=umbdata;Database=UmbracoDb;User Id=sa;Password=SQL_password123;",     "umbracoDbDSN_ProviderName": "Microsoft.Data.SqlClient"
    }

Finally we can compile a docker image for the Umbraco site.

    docker build --tag=UmbWeb ./UmbWeb

This will download the required components and compile a final image ready to run the site in a container, and may take some time. However before we are able to run both the site and the database container, we need to set up the network. 

At this point we can see all the images we have created by using the following command

    docker images

## 3.4 Running the website container in the same network

We can then run the website container. Notice in the command below there is an argument to let the container know which network to connect to.

    docker run --name UmbWeb -p 8000:80 -v umb_media:/app/wwwroot/media -v umb_logs:/app/umbraco/Logs -e ASPNETCORE_ENVIRONMENT='Staging' --network=umbNet -d UmbWeb

In the above command you can also see the volumes we use with the application container - specifically the log and the media folders. The reason to use these is that with media we want to share the media library if we should want to create more running sites (as we will later in the course) and with logs, we want to be able to view these logs and diagnose issues if the container isn't able to run for any reason.

One other thing we can see is the Environment variable we are passing the container with the -e flag, which sets our AspNetCore Environment to staging, and thus causes the container to run with the appsettings.staging.json file and allow us to connect to the database.

Once the container is running, if you run a docker ps command, you'll see both the database and website containers running.

    docker ps

![Running Containers](media/3_DockerPS.png)

You can also see the status of running containers and logs by running the Docker Desktop application.

# Slides - Networks and Volumes

Before we move to the next steps we will recap in more detail some of the steps we went through to get our site and database container up and running

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


# 4 Adding an API to the site

Now that there is a site and database running, we will add a simple REST API which will return a jason feed of the blog posts, which will be used in a later part of this workshop.

## 4.1 Creating the API controller

To save typing the code for the API is already created in the the /Files/UmbWeb folder. 

1. Copy the following whole folders from the /Files/UmbWeb folder to the /UmbWeb folder.
    - /Files/UmbWeb/Controllers to /UmbWeb/Controllers
    - /Files/UmbWeb/Models to /UmbWeb/Models

2. Amend the /UmbWeb/Startup.cs file so the ConfigureServices method resembles the following:
    
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(policy => 
            {
                policy.AddPolicy("CorsPolicy", opt => opt
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            // Bit of a mix - this also adds HTTP in the program.cs

            services.AddUmbraco(_env, _config)
                .AddBackOffice()
                .AddWebsite()
                .AddComposers()
                .Build();
        }  

3. Amend the /UmbWeb/Startup.cs file so the Configure method resembles the following:

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseUmbraco()
                .WithMiddleware(u =>
                {
                    u.UseBackOffice();
                    u.UseWebsite();
                })
                .WithEndpoints(u =>
                {
                    u.UseInstallerEndpoints();
                    u.UseBackOfficeEndpoints();
                    u.UseWebsiteEndpoints();
                });
        }
        

## 4.2 Rebuild the image and test the API

With those changes in there, you can re-build the UmbWeb image with the following command:

    docker build --tag=UmbWeb ./UmbWeb

We need to delete the existing running container before we can start the updated container, as docker will only allow once container with the same name at the same time. Run the following command in your terminal.

    docker rm -f UmbWeb

We can then re-start the UmbWeb container with the following command:

    docker run --name UmbWeb -p 8000:80 -v umb_media:/app/wwwroot/media -v umb_logs:/app/umbraco/Logs -e ASPNETCORE_ENVIRONMENT='Staging' --network=umbNet -d UmbWeb    

Once the container is running again we can check the API is working by browsing to the following URL:

    http://localhost:8000/Umbraco/Api/MyApp/GetBlogSummaries

This should return a JSON collection of Post Summaries in a collection, which we will use with the Blazor App.


## 4.3 Running a 2nd instance of the website container

While the website container has the API running, we want to spin up a 2nd instance of the website container. This will simulate a load-balanced environment.

    docker run --name UmbWeb2 -p 8001:80 -v umb_media:/app/wwwroot/media -v umb_logs:/app/umbraco/Logs -e ASPNETCORE_ENVIRONMENT='Staging' --network=umbNet -d UmbWeb 

You can browse this container by visiting the following URL:

    http://localhost:8001/

# 5 Add the Blazor Container

We will now create a final container which will be used to run a blazor app, connect to the Blog summary API and show a summary of posts.

## 5.1 Create the Blazor App and show the blog summaries

Start a new Blazor WASM project by running the following:

    dotnet new blazorwasm --name UmBlazor

Copy the following whole folders from the /Files/UmbWeb folder to the /UmbWeb folder.

- /Files/UmBlazor/Models to /UmBlazor/Models
- /Files/UmBlazor/Pages/FetchData.razor to /UmBlazor/Pages/FetchData.razor

Test that the application works by running the following command in your terminal:

    dotnet run --project UmBlazor
    
Ignoring any warnings, you should be able to browse the WASM site using the relevant output URL

![Blazor App](media/5_BlazorWasm.png)

In my case I can use https://localhost:7025. This will bring up a site, and the Fetch Data page should show the blog summaries from the Umbraco site.

![Blazor Fetch Data](media/5_BlazorWasm2.png)

## 5.2 Create the Blazor Container

To run the Blazor WASM app in a container, it's a little different to running an Umbraco website. The Umbraco site needs to run Kestrel as a webserver, but Blazor WASM just needs to serve files. As such it will use nginx to serve these pages on the container. 

I've created the Dockerfile and nginx configuration file, these need to be copied to the project folder

- /Files/UmBlazor/Dockerfile to /UmBlazor/Dockerfile
- /Files/UmBlazor/nginx.conf to /UmBlazor/nginx.conf
- /Files/UmBlazor/wwwroot/appsettings.json to /UmBlazor/wwwroot/appsettings.json

Looking at the contents of the Dockerfile : 

    FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
    WORKDIR /src
    COPY UmBlazor.csproj .
    RUN dotnet restore UmBlazor.csproj
    COPY . .
    RUN dotnet build UmBlazor.csproj -c Release -o /app/build

    FROM build AS publish
    RUN dotnet publish UmBlazor.csproj -c Release -o /app/publish

    FROM nginx:alpine AS final
    WORKDIR /usr/share/nginx/html
    COPY --from=publish /app/publish/wwwroot .
    COPY nginx.conf /etc/nginx/nginx.conf

We can see that this container is using the nginx image, and rather than starting the application, it merely hosts the published output of the UmBlazor project. 

### Build Image and run the container

Next we can build the Blazor Image using the following command:

    docker build --tag=umblazor .\UmBlazor    

Once that's done, we can run the Container

    docker run --name umblazor -p 8002:80 --network=umbNet -d umblazor

Now the site could be browsed using the containter using the url

    http://localhost:8002/

# 6 Docker compose

So far we have created all our containers manually but Docker, including all networks, volumes, ports. This could be scripted with a batch, but there's a cool tool called Docker Compose, which is a simple way to create and manage containers.

Slides 

- Docker Compose
    - Services
    - Networks
    - Volumes
    - Ports

## 6.1 Create the Docker Compose file

TODO : Expand on this. 

For now :

- Copy /Files/docker-compose.yml to /docker-compose.yml
- Copy /UmbWeb/appsettings.Staging.json to /UmbWeb/appsettings.Production.json
- Copy /UmBlazor/wwwroot/appsettings.Production.json to /UmBlazor/wwwroot/appsettings.Production.json

Todo : What's a better way to to Appsettings in Blazor?

## 6.2 Run the Docker Compose file

Finally before we run, we need to delete all existing containers. Run the following command in your terminal:

    docker rm -f umblazor UmbWeb UmbWeb2 umbdata

Verify that none are running by looking at the Docker Desktop app. Once confirming all running containers have been deleted, we can run the Docker Compose file

We first build the relevant images using the following command:

    docker compose build

That step isn't necessary, but it's good to have the images built before we run the containers. It also allows us to run all containers with the following command. 

    docker compose up

Once these are up, we can browse the umbraco websites using the following URLs

- Umbraco 1 : http://localhost:5080/ 
- Umbraco 2 : http://localhost:5081/
- Blazor : http://localhost:5082/

To stop the containers, run the following command:

    docker-compose down 

# References

## Umbraco

There is a great tool to help you configure the the unattended installation options for umbraco at [https://psw.codeshare.co.uk/](https://psw.codeshare.co.uk/)


## File Types

Remember, Linux line endings need -> Lf NOT CrLf

## Cleanup

## Troubleshooting 

Trust the Dev Certs

    dotnet dev-certs https --trust

Clear your local nuget cache

    dotnet nuget locals all --clear



