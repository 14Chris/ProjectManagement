#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 4433

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ProjectManagement.Api/ProjectManagement.Api.csproj", "ProjectManagement.Api/"]
COPY ["ProjectManagement.Models/ProjectManagement.Models.csproj", "ProjectManagement.Models/"]
RUN dotnet restore "ProjectManagement.Api/ProjectManagement.Api.csproj"
COPY . .
WORKDIR "/src/ProjectManagement.Api"
RUN dotnet build "ProjectManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectManagement.Api.dll"]
