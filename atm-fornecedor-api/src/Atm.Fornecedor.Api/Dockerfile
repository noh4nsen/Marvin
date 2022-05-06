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
COPY ["src/Atm.Fornecedor.Api/Atm.Fornecedor.Api.csproj", "src/Atm.Fornecedor.Api/"]
COPY ["src/Atm.Fornecedor.Dados/Atm.Fornecedor.Dados.csproj", "src/Atm.Fornecedor.Dados/"]
COPY ["src/Atm.Fornecedor/Atm.Fornecedor.csproj", "src/Atm.Fornecedor/"]
RUN dotnet restore "src/Atm.Fornecedor.Api/Atm.Fornecedor.Api.csproj"
COPY . .
WORKDIR "/src/src/Atm.Fornecedor.Api"
RUN dotnet build "Atm.Fornecedor.Api.csproj" -c ${BUILD_CONFIG} -o /app/build

FROM build AS publish
ARG BUILD_CONFIG
RUN dotnet publish "Atm.Fornecedor.Api.csproj" -c {BUILD_CONFIG} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Atm.Fornecedor.Api.dll"]
