# Use the SDK image to build and publish the website
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["UmbWeb.csproj", "."]
RUN dotnet restore "UmbWeb.csproj"
COPY . .
RUN dotnet publish "UmbWeb.csproj" -c Release -o /app/publish


# Copy the published output to the final running image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final 
WORKDIR /app

# Copy the media items to the final running image
COPY ./wwwroot/media ./media
# Copy the published output to the final running image
COPY --from=build /app/publish .


ENTRYPOINT ["dotnet", "UmbWeb.dll"]