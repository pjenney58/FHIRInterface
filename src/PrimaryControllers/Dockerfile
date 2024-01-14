#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV DOTNET_RUNNING_IN_DOCKER_CONTAINER=true

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/PrimaryControllers/Primary.csproj", "src/PrimaryControllers/"]
COPY ["src/PalisaidMeta/PalisaidMeta.csproj", "src/PalisaidMeta/"]
COPY ["src/Support/Support.csproj", "src/Support/"]
RUN dotnet restore "src/PrimaryControllers/Primary.csproj"
COPY . .
WORKDIR "/src/src/PrimaryControllers"
RUN dotnet build "Primary.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Primary.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Primary.dll"]
