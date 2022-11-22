#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["QuoVadis/QuoVadis.csproj", "QuoVadis/"]
COPY ["QuoVadis.Common/QuoVadis.Common.csproj", "QuoVadis.Common/"]
COPY ["QuoVadis.Contracts/QuoVadis.Contracts.csproj", "QuoVadis.Contracts/"]
COPY ["QuoVadis.GrainInterfaces/QuoVadis.GrainInterfaces.csproj", "QuoVadis.GrainInterfaces/"]
COPY ["QuoVadis.Grains/QuoVadis.Grains.csproj", "QuoVadis.Grains/"]
RUN dotnet restore "QuoVadis/QuoVadis.csproj"
COPY . .
WORKDIR "/src/QuoVadis"
RUN dotnet build "QuoVadis.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuoVadis.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuoVadis.dll"]