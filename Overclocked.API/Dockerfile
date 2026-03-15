FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src
COPY ["Overclocked.API/Overclocked.API.csproj", "Overclocked.API/"]
RUN dotnet restore "Overclocked.API/Overclocked.API.csproj"
COPY . .
WORKDIR "/src/Overclocked.API"
RUN dotnet build "Overclocked.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Overclocked.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Overclocked.API.dll"]