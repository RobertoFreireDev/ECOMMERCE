FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Company.Ecommerce/Company.Ecommerce.Host.csproj", "Company.Ecommerce/"]
COPY ["Company.Ecommerce.Common/Company.Ecommerce.Common.csproj", "Company.Ecommerce.Common/"]
COPY ["Company.Ecommerce.Events/Company.Ecommerce.Events.csproj", "Company.Ecommerce.Events/"]
COPY ["Company.Ecommerce.Orders.API/Company.Ecommerce.Orders.API.csproj", "Company.Ecommerce.Orders.API/"]
COPY ["Company.Ecommerce.Orders.Application/Company.Ecommerce.Orders.Application.csproj", "Company.Ecommerce.Orders.Application/"]
COPY ["Company.Ecommerce.Billing.API/Company.Ecommerce.Billing.API.csproj", "Company.Ecommerce.Billing.API/"]
COPY ["Company.Ecommerce.Billing.Application/Company.Ecommerce.Billing.Application.csproj", "Company.Ecommerce.Billing.Application/"]
COPY ["Company.Ecommerce.ShoppingCart.API/Company.Ecommerce.ShoppingCart.API.csproj", "Company.Ecommerce.ShoppingCart.API/"]

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