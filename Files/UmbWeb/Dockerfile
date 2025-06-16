# Use the SDK image to build and publish the website
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UmbWeb.csproj", "."]
RUN dotnet restore "UmbWeb.csproj"
COPY . .
RUN dotnet publish "UmbWeb.csproj" -c Release -o /app/publish

# Copy the published output to the final running image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final 
WORKDIR /app

# Expose the port that Umbraco will run on
ENV ASPNETCORE_URLS=http://+:8081

# Copy the published output to the final running image
COPY --from=build /app/publish .

# Copy the media items to the final running image
# THIS IS NOT WHAT YOU WANT TO DO IN PRODUCTION, but for the sake of this workshop we will copy the media items into the container. In Production you would use a volume to store the media items, or a shared storage solution.
COPY ./wwwroot/media ./wwwroot/media

# Set the entrypoint to the web application
ENTRYPOINT ["dotnet", "UmbWeb.dll"]