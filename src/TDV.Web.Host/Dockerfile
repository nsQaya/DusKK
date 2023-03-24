#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/TDV.Web.Host/TDV.Web.Host.csproj", "src/TDV.Web.Host/"]
COPY ["src/TDV.Web.Core/TDV.Web.Core.csproj", "src/TDV.Web.Core/"]
COPY ["src/TDV.Application/TDV.Application.csproj", "src/TDV.Application/"]
COPY ["src/TDV.Application.Shared/TDV.Application.Shared.csproj", "src/TDV.Application.Shared/"]
COPY ["src/TDV.Core.Shared/TDV.Core.Shared.csproj", "src/TDV.Core.Shared/"]
COPY ["src/TDV.Core/TDV.Core.csproj", "src/TDV.Core/"]
COPY ["src/TDV.EntityFrameworkCore/TDV.EntityFrameworkCore.csproj", "src/TDV.EntityFrameworkCore/"]
COPY ["src/TDV.GraphQL/TDV.GraphQL.csproj", "src/TDV.GraphQL/"]
RUN dotnet restore "src/TDV.Web.Host/TDV.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/TDV.Web.Host"
RUN dotnet build "TDV.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TDV.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TDV.Web.Host.dll"]
