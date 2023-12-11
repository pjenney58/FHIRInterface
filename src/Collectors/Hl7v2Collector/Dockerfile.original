#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV DOTNET_RUNNING_IN_DOCKER_CONTAINER=true

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Collectors/Hl7Collector/Hl7Collector.csproj", "src/Collectors/Hl7Collector/"]
RUN dotnet restore "src/Collectors/Hl7Collector/Hl7Collector.csproj"
COPY . .
WORKDIR "/src/src/Collectors/Hl7Collector"
RUN dotnet build "Hl7Collector.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hl7Collector.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hl7Collector.dll"]
