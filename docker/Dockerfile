FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore TFA.Forums.API
RUN dotnet publish -c Release -o out TFA.Forums.API

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

CMD dotnet TFA.Forums.API.dll