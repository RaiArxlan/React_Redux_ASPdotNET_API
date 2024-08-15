#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["React_Redux_ASPdotNET_API.Server/React_Redux_ASPdotNET_API.Server.csproj", "React_Redux_ASPdotNET_API.Server/"]
RUN dotnet restore "./React_Redux_ASPdotNET_API.Server/React_Redux_ASPdotNET_API.Server.csproj"
COPY . .
WORKDIR "/src/React_Redux_ASPdotNET_API.Server"
RUN dotnet build "./React_Redux_ASPdotNET_API.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./React_Redux_ASPdotNET_API.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "React_Redux_ASPdotNET_API.Server.dll"]