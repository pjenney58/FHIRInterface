#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV DOTNET_RUNNING_IN_DOCKER_CONTAINER=true

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Collectors/X12Collector/X12Collector.csproj", "src/Collectors/X12Collector/"]
RUN dotnet restore "src/Collectors/X12Collector/X12Collector.csproj"
COPY . .
WORKDIR "/src/src/Collectors/X12Collector"
RUN dotnet build "X12Collector.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "X12Collector.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "X12Collector.dll"]
