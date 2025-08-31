FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7141
EXPOSE 7141

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["YARP_ReverseProxy/YARP_ReverseProxy.csproj", "YARP_ReverseProxy/"]
RUN dotnet restore "YARP_ReverseProxy/YARP_ReverseProxy.csproj"
COPY . .
WORKDIR "/src/YARP_ReverseProxy"
RUN dotnet build "YARP_ReverseProxy.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "YARP_ReverseProxy.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YARP_ReverseProxy.dll"]
