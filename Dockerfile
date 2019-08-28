FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ARG dbServer
ARG dbUsername
ARG dbPassword
ARG ASPNETCORE_ENVIRONMENT
ARG ASPNETCORE_URLS
ARG ASPNETCORE_HTTPS_PORT

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["ProTrack/ProTrack.csproj", "ProTrack/"]
COPY ["ProTrack.Data/ProTrack.Data.csproj", "ProTrack.Data/"]
COPY ["ProTrack.Service/ProTrack.Service.csproj", "ProTrack.Service/"]
RUN dotnet restore "ProTrack/ProTrack.csproj"
COPY . .
WORKDIR "/src/ProTrack"
RUN dotnet build "ProTrack.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ProTrack.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ProTrack.dll"]