# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/HLS.Topup.Web.Host/HLS.Topup.Web.Host.csproj", "src/HLS.Topup.Web.Host/"]
COPY ["src/HLS.Topup.EventBus/HLS.Topup.EventBus.csproj", "src/HLS.Topup.EventBus/"]
COPY ["src/HLS.Topup.Core/HLS.Topup.Core.csproj", "src/HLS.Topup.Core/"]
COPY ["src/HLS.Topup.Application.Shared/HLS.Topup.Application.Shared.csproj", "src/HLS.Topup.Application.Shared/"]
COPY ["src/HLS.Topup.Core.Shared/HLS.Topup.Core.Shared.csproj", "src/HLS.Topup.Core.Shared/"]
COPY ["src/HLS.Topup.Web.Core/HLS.Topup.Web.Core.csproj", "src/HLS.Topup.Web.Core/"]
COPY ["src/HLS.Topup.Application/HLS.Topup.Application.csproj", "src/HLS.Topup.Application/"]
COPY ["src/HLS.Topup.EntityFrameworkCore/HLS.Topup.EntityFrameworkCore.csproj", "src/HLS.Topup.EntityFrameworkCore/"]
COPY ["src/HLS.Topup.GraphQL/HLS.Topup.GraphQL.csproj", "src/HLS.Topup.GraphQL/"]
RUN dotnet restore "./src/HLS.Topup.Web.Host/HLS.Topup.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/HLS.Topup.Web.Host"
RUN dotnet build "./HLS.Topup.Web.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HLS.Topup.Web.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HLS.Topup.Web.Host.dll"]