#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ProjectManagment.Api/ProjectManagment.Api.csproj", "ProjectManagment.Api/"]
COPY ["ProjectManagment.Models/ProjectManagment.Models.csproj", "ProjectManagment.Models/"]
RUN dotnet restore "ProjectManagment.Api/ProjectManagment.Api.csproj"
COPY . .
WORKDIR "/src/ProjectManagment.Api"
RUN dotnet build "ProjectManagment.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectManagment.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectManagment.Api.dll"]