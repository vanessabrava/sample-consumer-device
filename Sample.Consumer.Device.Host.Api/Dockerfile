FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY *.sln ./
COPY NuGet.config ./
COPY Sample.Consumer.Device.Host.Api/Sample.Consumer.Device.Host.Api.csproj Sample.Consumer.Device.Host.Api/
RUN dotnet restore Sample.Consumer.Device.Host.Api/Sample.Consumer.Device.Host.Api.csproj

COPY . .
WORKDIR /src/Sample.Consumer.Device.Host.Api
RUN dotnet build Sample.Consumer.Device.Host.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Sample.Consumer.Device.Host.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.Consumer.Device.Host.Api.dll"]
