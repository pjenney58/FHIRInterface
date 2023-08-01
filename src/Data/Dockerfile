#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Data/Data.csproj", "src/Data/"]
RUN dotnet restore "src/Data/Data.csproj"
COPY . .
WORKDIR "/src/src/Data"
RUN dotnet build "Data.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Data.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Data.dll"]
