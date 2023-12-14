#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Collectors/Hl7v2Collector/Hl7v2Collector.csproj", "src/Collectors/Hl7v2Collector/"]
COPY ["src/Collectors/Collector/Collector.csproj", "src/Collectors/Collector/"]
COPY ["src/DataShapes/DataShapes.csproj", "src/DataShapes/"]
COPY ["src/Transporters/Transporter/Transporter.csproj", "src/Transporters/Transporter/"]
COPY ["src/Transporters/MLLPTransporter/MLLPTransporter.csproj", "src/Transporters/MLLPTransporter/"]
COPY ["src/Support/Support.csproj", "src/Support/"]
RUN dotnet restore "./src/Collectors/Hl7v2Collector/./Hl7v2Collector.csproj"
COPY . .
WORKDIR "/src/src/Collectors/Hl7v2Collector"
RUN dotnet build "./Hl7v2Collector.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Hl7v2Collector.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hl7v2Collector.dll"]