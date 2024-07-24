#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 81

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release

#Add GitHub NuGet sources
ARG GITHUB_PASSWORD

WORKDIR /src
COPY ["src/API/Notifications.API/Notifications.API.csproj", "src/API/Notifications.API/"]
COPY ["src/Infrastructure/Notifications.GrpcServer/Notifications.GrpcServer.csproj", "src/Infrastructure/Notifications.GrpcServer/"]
COPY ["src/Infrastructure/Notifications.MessageProcessor/Notifications.MessageProcessor.csproj", "src/Infrastructure/Notifications.MessageProcessor/"]
RUN dotnet nuget add source -n github -u AZ -p ${GITHUB_PASSWORD} --store-password-in-clear-text https://nuget.pkg.github.com/prafaelmsantos/index.json
RUN dotnet restore "./src/API/Notifications.API/Notifications.API.csproj"
COPY . .
WORKDIR "/src/src/API/Notifications.API"
RUN dotnet build "./Notifications.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Notifications.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notifications.API.dll"]