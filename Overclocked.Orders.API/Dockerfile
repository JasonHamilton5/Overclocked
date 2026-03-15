FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src
COPY ["Overclocked.Orders.API/Overclocked.Orders.API.csproj", "Overclocked.Orders.API/"]
RUN dotnet restore "Overclocked.Orders.API/Overclocked.Orders.API.csproj"
COPY . .
WORKDIR "/src/Overclocked.Orders.API"
RUN dotnet build "Overclocked.Orders.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Overclocked.Orders.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Overclocked.Orders.API.dll"]