# 7 Add the Blazor Container

We will now create a final container which will be used to run a blazor app, connect to the Blog summary API and show a summary of posts.

## 7.1 Create the Blazor App and show the blog summaries

***Action:*** Start a new Blazor WASM project by running the following:

```bash
dotnet new blazorwasm --name UmBlazor

## Add the project to the solution file
dotnet sln Umbraco-Docker-Workshop.sln add "UmBlazor/UmBlazor.csproj"
```

***Action:*** Copy the following whole folders from the /Files/UmbWeb folder to the /Workshop/UmbWeb folder.

- **/Files/UmBlazor/Models** to **/Workshop/UmBlazor/Models**
- **/Files/UmBlazor/Pages/FetchData.razor** to **/Workshop/UmBlazor/Pages/FetchData.razor**
- **/Files/UmBlazor/Layout/NavMenu.razor** to **/Workshop/UmBlazor/Layout/NavMenu.razor**
- **/Files/UmBlazor/wwwroot/appsettings.json** to **/Workshop/UmBlazor/wwwroot/appsettings.json**

***Action:*** Test that the application works by running the following command in your terminal:

```bash
    dotnet run --project UmBlazor
```
    
Ignoring any warnings, you should be able to browse the WASM site using the relevant output URL

![Blazor App](media/5_BlazorWasm.png)

In my case I can use https://localhost:7025. This will bring up a site, and the Fetch Data page should show the blog summaries from the Umbraco site.

![Blazor Fetch Data](media/5_BlazorWasm2.png)


## 7.2 Genrate the HttpClient Code

```bash

dotnet tool install --global NSwag.ConsoleCore --version 13.20.0

nswag openapi2csclient /input:http://localhost:8000/umbraco/swagger/delivery/swagger.json /output:ApiClient.cs /namespace:UmBlazor.Clients
```




## 7.2 Create the Blazor Container

To run the Blazor WASM app in a container, it's a little different to running an Umbraco website. The Umbraco site needs to run Kestrel as a webserver, but Blazor WASM just needs to serve files. As such it will use nginx to serve these pages on the container. 

I've created the Dockerfile and nginx configuration file, these need to be copied to the project folder.

***Action:*** Copy the following:

- **/Files/UmBlazor/Dockerfile** to **/Workshop/UmBlazor/Dockerfile**
- **/Files/UmBlazor/nginx.conf** to **/Workshop/UmBlazor/nginx.conf**

*Note : Don't copy the other files yet*

Looking at the contents of the Dockerfile : 

```dockerfile
    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
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
```

We can see that this container is using the nginx image, and rather than starting the application, it merely hosts the published output of the UmBlazor project. 

### Build Image and run the container

Next we can build the Blazor Image using the following command:

***Action:*** Run the following command to build the image.

```bash
docker build --tag=umblazor .\UmBlazor    
```

Once that's done, we can run the Container

***Action:*** Run the following command to start the container.

```bash
docker run --name umblazor -p 8002:80 --network=umbNet -d umblazor
```

Now the site could be browsed using the containter using the url [http://localhost:8002/](http://localhost:8002/).
   
Upon running the site we should see the same Blazor app from the earlier example, but this time running from the container instance, and when it queries the rest API to load blog content, it is doing so from the content delivery container.

