# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/core/sdk AS build-env
WORKDIR /app

# copy csproj
COPY weatherapplication/*.csproj ./weatherapplication/
RUN dotnet restore weatherapplication/*.csproj

# copy everything else and build app
COPY weatherapplication/ ./weatherapplication/
WORKDIR /app/weatherapplication
RUN dotnet publish -c Release -o out

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet
WORKDIR /app
COPY --from=build-env /app/weatherapplication/out ./
ENTRYPOINT ["dotnet", "weatherapplication.dll"]

# Source: https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-7.0
# https://softchris.github.io/pages/dotnet-dockerize.html#create-a-dockerfile
