FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src
COPY ["Overclocked.Products.API/Overclocked.Products.API.csproj", "Overclocked.Products.API/"]
RUN dotnet restore "Overclocked.Products.API/Overclocked.Products.API.csproj"
COPY . .
WORKDIR "/src/Overclocked.Products.API"
RUN dotnet build "Overclocked.Products.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Overclocked.Products.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Overclocked.Products.API.dll"]