# взято с https://stackoverflow.com/questions/51769324/how-to-create-run-net-core-console-app-in-docker
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ExchangeRatesConsole.csproj .
RUN dotnet restore ExchangeRatesConsole.csproj

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "ExchangeRatesConsole.dll"]