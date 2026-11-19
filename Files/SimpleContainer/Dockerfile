# Use the SDK image to build and publish the website
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["SimpleContainer.csproj", "."]
RUN dotnet restore "SimpleContainer.csproj"
COPY . .
RUN dotnet publish "SimpleContainer.csproj" -c Release -o /app/publish

# Copy the published output to the final running image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
# Set the environment to Development, so that Umbraco loads the connection string from the appsettings.Development.json file
ENV ASPNETCORE_ENVIRONMENT=Development
# Set the port that Umbraco will listen on inside the container
ENV ASPNETCORE_HTTP_PORTS=8081
# Document the port that the application will run on
# The container serves plain HTTP, so allow backoffice logins without HTTPS
ENV Umbraco__CMS__Global__UseHttps=false
EXPOSE 8081

# Copy the published output to the final running image
COPY --from=build /app/publish .

# Set the entrypoint to the web application
ENTRYPOINT ["dotnet", "SimpleContainer.dll"]
