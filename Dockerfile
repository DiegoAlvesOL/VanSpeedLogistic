# Estágio 1: Build (Usando SDK 9.0 oficial)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Copia o arquivo .csproj da subpasta para o container
COPY ["VanSpeedLogistics/VanSpeedLogistics.csproj", "VanSpeedLogistics/"]

# 2. Restaura as dependências
RUN dotnet restore "VanSpeedLogistics/VanSpeedLogistics.csproj"

# 3. Copia todo o restante dos arquivos
COPY . .

# 4. Entra na pasta do projeto e compila
WORKDIR "/src/VanSpeedLogistics"
RUN dotnet publish "VanSpeedLogistics.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio 2: Runtime (Ambiente de execução leve)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Variáveis de ambiente para o Railway
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "VanSpeedLogistics.dll"]