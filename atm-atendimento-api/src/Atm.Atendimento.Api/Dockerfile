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
COPY ["src/Atm.Atendimento.Api/Atm.Atendimento.Api.csproj", "src/Atm.Atendimento.Api/"]
COPY ["src/Atm.Atendimento.Dados/Atm.Atendimento.Dados.csproj", "src/Atm.Atendimento.Dados/"]
COPY ["src/Atm.Atendimento/Atm.Atendimento.csproj", "src/Atm.Atendimento/"]
RUN dotnet restore "src/Atm.Atendimento.Api/Atm.Atendimento.Api.csproj"
COPY . .
WORKDIR "/src/src/Atm.Atendimento.Api"
RUN dotnet build "Atm.Atendimento.Api.csproj" -c ${BUILD_CONFIG} -o /app/build

FROM build AS publish
ARG BUILD_CONFIG
RUN dotnet publish "Atm.Atendimento.Api.csproj" -c {BUILD_CONFIG} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Atm.Atendimento.Api.dll"]
