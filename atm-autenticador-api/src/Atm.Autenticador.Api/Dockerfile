#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
ARG HTTP_PORT
ARG HTTPS_PORT
COPY ["certificates/*", "https/"]
WORKDIR /app
EXPOSE ${HTTP_PORT} ${HTTPS_PORT}

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BUILD_CONFIG
WORKDIR /src
COPY ["src/Atm.Autenticador.Api/Atm.Autenticador.Api.csproj", "src/Atm.Autenticador.Api/"]
COPY ["src/Atm.Autenticador.Dados/Atm.Autenticador.Dados.csproj", "src/Atm.Autenticador.Dados/"]
COPY ["src/Atm.Autenticador/Atm.Autenticador.csproj", "src/Atm.Autenticador/"]
RUN dotnet restore "src/Atm.Autenticador.Api/Atm.Autenticador.Api.csproj"
COPY . .
WORKDIR "/src/src/Atm.Autenticador.Api"
RUN dotnet build "Atm.Autenticador.Api.csproj" -c ${BUILD_CONFIG} -o /app/build

FROM build AS publish
ARG BUILD_CONFIG
RUN dotnet publish "Atm.Autenticador.Api.csproj" -c {BUILD_CONFIG} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Atm.Autenticador.Api.dll"]
