#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["./services/NSE.Identity.API/NSE.Identity.API.csproj", "services/NSE.Identity.API/"]
COPY ["./building.blocks/NSE.Core/NSE.Core.csproj", "building.blocks/NSE.WebAPI.Core/"]
COPY ["./building.blocks/NSE.WebApi.Core/NSE.WebApi.Core.csproj", "building.blocks/NSE.WebApi.Core/NSE.WebApi.Core.csproj"]
COPY ["./building.blocks/NSE.MessageBus/NSE.MessageBus.csproj", "building.blocks/NSE.MessageBus/"]
RUN dotnet restore "src/services/NSE.Identity.API/NSE.Identity.API.csproj"
COPY . .
WORKDIR "services/NSE.Identity.API"
RUN dotnet build "NSE.Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NSE.Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NSE.Identity.API.dll"]