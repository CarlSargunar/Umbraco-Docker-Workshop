FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY UmBlazor.csproj .
RUN dotnet restore UmBlazor.csproj
COPY . .
RUN dotnet build UmBlazor.csproj -c Release -o /app/build

FROM build AS publish
# Blazor WebAssembly runs in the browser, so its environment (which decides which
# appsettings.{Environment}.json file is loaded) is baked in when the app is published.
ARG BLAZOR_ENVIRONMENT=Production
RUN dotnet publish UmBlazor.csproj -c Release -o /app/publish -p:WasmApplicationEnvironmentName=$BLAZOR_ENVIRONMENT

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf
