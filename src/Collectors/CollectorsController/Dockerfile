#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV DOTNET_RUNNING_IN_DOCKER_CONTAINER=true

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Collectors/CollectorsController/CollectorsController.csproj", "src/Collectors/CollectorsController/"]
RUN dotnet restore "src/Collectors/CollectorsController/CollectorsController.csproj"
COPY . .
WORKDIR "/src/src/Collectors/CollectorsController"
RUN dotnet build "CollectorsController.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CollectorsController.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CollectorsController.dll"]
