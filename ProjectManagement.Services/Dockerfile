FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY /ProjectManagement.Api/*.csproj ProjectManagement.Api/
COPY /ProjectManagement.Models/*.csproj ProjectManagement.Models/
RUN dotnet restore

# Copy everything else and build
COPY ./ProjectManagement.Api ./ProjectManagement.Api
COPY ./ProjectManagement.Models ./ProjectManagement.Models
WORKDIR /app/ProjectManagement.Api
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/ProjectManagement.Api/out .
ENTRYPOINT ["dotnet", "ProjectManagement.Api.dll"]