FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Company.Ecommerce/Company.Ecommerce.Host.csproj", "Company.Ecommerce/"]
COPY ["Company.Ecommerce.Orders/Company.Ecommerce.Orders.csproj", "Company.Ecommerce.Orders/"]

RUN dotnet restore "Company.Ecommerce/Company.Ecommerce.Host.csproj"

COPY . .
WORKDIR "/src/Company.Ecommerce"
RUN dotnet build "./Company.Ecommerce.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Company.Ecommerce.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Company.Ecommerce.Host.dll"]