# Use the SDK image to build and publish the website
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SimpleContainer.csproj", "."]
RUN dotnet restore "SimpleContainer.csproj"
COPY . .
RUN dotnet publish "SimpleContainer.csproj" -c Release -o /app/publish

# Copy the published output to the final running image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final 
WORKDIR /app
# Set the environment to Development, so that Umbraco loads the connection string from the appsettings.developement.json file
ENV ASPNETCORE_ENVIRONMENT=Development
# Expose the port that Umbraco will run on
ENV ASPNETCORE_URLS=http://+:8081
# Expose the port that the application will run on
EXPOSE 8081

# Copy the published output to the final running image
COPY --from=build /app/publish .

# Copy the media items to the final running image
COPY ./wwwroot/media ./wwwroot/media

# Set the entrypoint to the web application
ENTRYPOINT ["dotnet", "SimpleContainer.dll"]