#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/HLS.Topup.Migrator/HLS.Topup.Migrator.csproj", "src/HLS.Topup.Migrator/"]
COPY ["src/HLS.Topup.EntityFrameworkCore/HLS.Topup.EntityFrameworkCore.csproj", "src/HLS.Topup.EntityFrameworkCore/"]
COPY ["src/HLS.Topup.Core/HLS.Topup.Core.csproj", "src/HLS.Topup.Core/"]
COPY ["src/HLS.Topup.Core.Shared/HLS.Topup.Core.Shared.csproj", "src/HLS.Topup.Core.Shared/"]
RUN dotnet restore "src/HLS.Topup.Migrator/HLS.Topup.Migrator.csproj"
COPY . .
WORKDIR "/src/src/HLS.Topup.Migrator"
RUN dotnet build "HLS.Topup.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HLS.Topup.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HLS.Topup.Migrator.dll"]
