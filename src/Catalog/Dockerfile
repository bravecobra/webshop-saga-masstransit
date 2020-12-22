#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Catalog/Catalog.csproj", "Catalog/"]
COPY ["Webshop.Shared/Webshop.Shared.csproj", "Webshop.Shared/"]
COPY ["Webshop.Shared.Ddd/Webshop.Shared.Ddd.csproj", "Webshop.Shared.Ddd/"]
COPY ["Webshop.Shared.Infrastructure.Consul/Webshop.Shared.Infrastructure.Consul.csproj", "Webshop.Shared.Infrastructure.Consul/"]
RUN dotnet restore "Catalog/Catalog.csproj"
COPY . .
WORKDIR "/src/Catalog"
RUN dotnet build "Catalog.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.dll"]