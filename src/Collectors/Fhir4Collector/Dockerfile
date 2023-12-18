#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["src/Collectors/Fhir4Collector/Fhir4Collector.csproj", "src/Collectors/Fhir4Collector/"]
COPY ["src/Transformers/Transformer/Transformer.csproj", "src/Transformers/Transformer/"]
COPY ["src/DataShapes/DataShapes.csproj", "src/DataShapes/"]
COPY ["src/Support/Support.csproj", "src/Support/"]
COPY ["src/Transformers/TransformerFactory/TransformerFactory.csproj", "src/Transformers/TransformerFactory/"]
COPY ["src/Collectors/Collector/Collector.csproj", "src/Collectors/Collector/"]
COPY ["src/Transporters/Transporter/Transporter.csproj", "src/Transporters/Transporter/"]
RUN dotnet restore "./src/Collectors/Fhir4Collector/./Fhir4Collector.csproj"
COPY . .
WORKDIR "/src/src/Collectors/Fhir4Collector"
RUN dotnet build "./Fhir4Collector.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./Fhir4Collector.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fhir4Collector.dll"]