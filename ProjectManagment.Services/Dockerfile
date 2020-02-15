FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY /ProjectManagment.Api/*.csproj ProjectManagment.Api/
COPY /ProjectManagment.Models/*.csproj ProjectManagment.Models/
RUN dotnet restore

# Copy everything else and build
COPY ./ProjectManagment.Api ./ProjectManagment.Api
COPY ./ProjectManagment.Models ./ProjectManagment.Models
WORKDIR /app/ProjectManagment.Api
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/ProjectManagment.Api/out .
ENTRYPOINT ["dotnet", "ProjectManagment.Api.dll"]