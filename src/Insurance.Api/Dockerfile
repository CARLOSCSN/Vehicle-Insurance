# Multi-stage build para otimizar o tamanho da imagem

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de projeto
COPY ["src/Insurance.Api/Insurance.Api.csproj", "src/Insurance.Api/"]
COPY ["src/Insurance.Application/Insurance.Application.csproj", "src/Insurance.Application/"]
COPY ["src/Insurance.Domain/Insurance.Domain.csproj", "src/Insurance.Domain/"]
COPY ["src/Insurance.Infrastructure/Insurance.Infrastructure.csproj", "src/Insurance.Infrastructure/"]

# Restaurar dependências
RUN dotnet restore "src/Insurance.Api/Insurance.Api.csproj"

# Copiar todo o código
COPY . .

# Build
WORKDIR "/src/src/Insurance.Api"
RUN dotnet build "Insurance.Api.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Insurance.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Criar diretório para o banco de dados
RUN mkdir -p /app/data

# Copiar arquivos publicados
COPY --from=publish /app/publish .

# Expor portas
EXPOSE 80
EXPOSE 443

# Variáveis de ambiente
ENV ASPNETCORE_URLS=http://+:80
ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/insurance.db"

# Healthcheck
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl --fail http://localhost:80/health || exit 1

# Ponto de entrada
ENTRYPOINT ["dotnet", "Insurance.Api.dll"]
