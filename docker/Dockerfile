FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore src/TFA.Forums.API
RUN dotnet publish -c Release -o out src/TFA.Forums.API

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

CMD ["dotnet", "src/TFA.Forums.API.dll"]