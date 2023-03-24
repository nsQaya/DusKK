#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/TDV.Migrator/TDV.Migrator.csproj", "src/TDV.Migrator/"]
COPY ["src/TDV.EntityFrameworkCore/TDV.EntityFrameworkCore.csproj", "src/TDV.EntityFrameworkCore/"]
COPY ["src/TDV.Core/TDV.Core.csproj", "src/TDV.Core/"]
COPY ["src/TDV.Core.Shared/TDV.Core.Shared.csproj", "src/TDV.Core.Shared/"]
RUN dotnet restore "src/TDV.Migrator/TDV.Migrator.csproj"
COPY . .
WORKDIR "/src/src/TDV.Migrator"
RUN dotnet build "TDV.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TDV.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TDV.Migrator.dll", "-s"]
