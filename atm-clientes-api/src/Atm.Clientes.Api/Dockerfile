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
COPY ["src/Atm.Clientes.Api/Atm.Clientes.Api.csproj", "src/Atm.Clientes.Api/"]
COPY ["src/Atm.Clientes.Dados/Atm.Clientes.Dados.csproj", "src/Atm.Clientes.Dados/"]
COPY ["src/Atm.Clientes/Atm.Clientes.csproj", "src/Atm.Clientes/"]
RUN dotnet restore "src/Atm.Clientes.Api/Atm.Clientes.Api.csproj"
COPY . .
WORKDIR "/src/src/Atm.Clientes.Api"
RUN dotnet build "Atm.Clientes.Api.csproj" -c ${BUILD_CONFIG} -o /app/build

FROM build AS publish
ARG BUILD_CONFIG
RUN dotnet publish "Atm.Clientes.Api.csproj" -c {BUILD_CONFIG} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Atm.Clientes.Api.dll"]
