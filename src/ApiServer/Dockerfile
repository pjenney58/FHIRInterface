#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/ApiServer/ApiServer.csproj", "src/ApiServer/"]
RUN dotnet restore "src/ApiServer/ApiServer.csproj"
COPY . .
WORKDIR "/src/src/ApiServer"
RUN dotnet build "ApiServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiServer.dll"]