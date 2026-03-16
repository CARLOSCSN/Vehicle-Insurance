# 🚗 Sistema de Seguros de Veículos

Sistema completo de cálculo e gestão de seguros de veículos desenvolvido com **.NET 8** e **Clean Architecture**.

## 📋 Sobre o Projeto

Este projeto implementa um sistema de seguros de veículos com cálculo automático de prêmios baseado no valor do veículo. O sistema foi desenvolvido seguindo os princípios da **Clean Architecture** e inclui testes automatizados, API RESTful e interface web moderna.

### Fórmulas de Cálculo

O sistema utiliza as seguintes fórmulas para calcular os valores do seguro:

```
Taxa de Risco = (Valor do Veículo * 5) / (2 x Valor do Veículo) = 2,5%
Prêmio de Risco = Taxa de Risco * Valor do Veículo
Prêmio Puro = Prêmio de Risco * (1 + MARGEM_SEGURANÇA)
Prêmio Comercial = LUCRO * Prêmio Puro

Onde:
- MARGEM_SEGURANÇA = 3%
- LUCRO = 5%
```

**Exemplo:** Para um veículo de R$ 10.000,00:
- Taxa de Risco = 2,5%
- Prêmio de Risco = R$ 250,00
- Prêmio Puro = R$ 257,50
- **Valor do Seguro = R$ 270,37**

## 🏗️ Arquitetura

O projeto segue os princípios da **Clean Architecture** dividida em camadas:

```
📦 VehicleInsurance
├── 📂 src
│   ├── 📂 Insurance.Api          # API RESTful (Controllers, Middleware)
│   ├── 📂 Insurance.Application  # Casos de Uso e Serviços de Aplicação
│   ├── 📂 Insurance.Domain       # Entidades, Value Objects, Interfaces
│   └── 📂 Insurance.Infrastructure # Repositórios, Banco de Dados, Serviços Externos
└── 📂 tests
    └── 📂 Insurance.Tests        # Testes Unitários
```

### Camadas

- **Domain**: Core do negócio (Entidades, Value Objects, Interfaces)
- **Application**: Orquestração de casos de uso (DTOs, Serviços, Validadores)
- **Infrastructure**: Implementações técnicas (EF Core, SQL Server, Repositórios)
- **API**: Camada de apresentação (Controllers, Middleware, Configurações)

## 🚀 Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **ASP.NET Core Web API** - API RESTful
- **Entity Framework Core** - ORM para acesso a dados
- **SQL Server 2022** - Banco de dados relacional
- **FluentValidation** - Validação de requisições
- **xUnit** - Framework de testes unitários
- **Docker & Docker Compose** - Containerização
- **Swagger/OpenAPI** - Documentação da API
- **Chart.js** - Gráficos interativos no front-end

## 📦 Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Editor: Visual Studio 2022, VS Code ou Rider

## 🔧 Como Executar

### Opção 1: Com Docker Compose (Recomendado)

```bash
# Clone o repositório
git clone <repository-url>
cd VehicleInsurance

# Inicie os containers (SQL Server + API)
docker-compose up -d

# Aguarde alguns segundos para o SQL Server inicializar
# A API estará disponível em:
# - http://localhost:5000
# - Swagger: http://localhost:5000/swagger
# - Dashboard: http://localhost:5000/index.html
```

### Opção 2: Executar Localmente

```bash
# 1. Iniciar SQL Server via Docker
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Insurance@123" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest

# 2. Restaurar pacotes
dotnet restore

# 3. Executar as migrations
dotnet ef database update --project src/Insurance.Infrastructure --startup-project src/Insurance.Api

# 4. Executar a aplicação
dotnet run --project src/Insurance.Api

# A API estará disponível em http://localhost:5000
```

### Opção 3: Limpeza e Rebuild Completo

Se encontrar problemas com DLLs bloqueadas ou erros de compilação:

```powershell
# No PowerShell como Administrador
powershell -ExecutionPolicy Bypass .\clean-rebuild.ps1
```

Este script irá:
1. Limpar a solução
2. Remover pastas `bin` e `obj`
3. Restaurar pacotes NuGet
4. Recompilar o projeto
5. Desbloquear DLLs bloqueadas pelo Windows

## 🔧 Troubleshooting

Encontrou problemas? Consulte nosso [**Guia de Resolução de Problemas (TROUBLESHOOTING.md)**](TROUBLESHOOTING.md) que contém soluções para:

- ❌ Erro de Política de Controle de Aplicativo bloqueando DLLs
- ❌ Erro no Docker com SQL Server (dependency failed)
- ❌ Porta já em uso
- ❌ Falha na conexão com banco de dados

### Scripts Auxiliares

O projeto inclui scripts PowerShell para facilitar o desenvolvimento:

```powershell
# Limpar e recompilar completamente
.\clean-rebuild.ps1

# Desbloquear DLLs bloqueadas pelo Windows
.\unblock-dlls.ps1
```

## 📚 Endpoints da API

### POST /api/insurance
Cria uma nova apólice de seguro

**Request:**
```json
{
  "insuredCpf": "12345678900",
  "vehicleModel": "Honda Civic 2024",
  "vehicleValue": 120000.00
}
```

**Response:**
```json
{
  "id": 1,
  "insuredName": "João Silva",
  "insuredCpf": "12345678900",
  "insuredAge": 35,
  "vehicleModel": "Honda Civic 2024",
  "vehicleValue": 120000.00,
  "riskRate": 2.5,
  "riskPremium": 3000.00,
  "purePremium": 3090.00,
  "commercialPremium": 154.50,
  "insuranceValue": 3244.50,
  "createdAt": "2026-03-15T22:00:00Z"
}
```

### GET /api/insurance/{id}
Busca uma apólice por ID

### GET /api/insurance
Lista todas as apólices de seguro

### GET /api/insurance/report
Retorna relatório com médias

**Response:**
```json
{
  "averageInsuranceValue": 1500.25,
  "averageVehicleValue": 55000.00,
  "totalPolicies": 15
}
```

### GET /api/external/insured/{cpf}
Mock do serviço externo de dados do segurado

## 🌐 Interface Web

Acesse o dashboard moderno em `http://localhost:5000/index.html`

**Funcionalidades:**
- 📊 Cards com estatísticas principais
- 📈 Gráficos interativos (Chart.js)
- 📋 Tabela com lista de apólices
- 🔄 Atualização automática a cada 30 segundos
- 📱 Design responsivo

## 🧪 Testes

```bash
# Executar todos os testes
dotnet test

# Executar com cobertura
dotnet test /p:CollectCoverage=true /p:CoverageReporter=html
```

**Testes implementados:**
- ✅ Testes do calculador de seguros
- ✅ Testes de validação de CPF
- ✅ Testes de Value Objects (Money, CPF)
- ✅ Testes de validadores FluentValidation
- ✅ Testes do domínio (PremiumCalculator)

## 📊 Banco de Dados

O projeto utiliza **SQL Server 2022** com **Entity Framework Core Code-First**.

### Migrations

```bash
# Adicionar nova migration
dotnet ef migrations add NomeDaMigration --project src/Insurance.Infrastructure --startup-project src/Insurance.Api

# Aplicar migrations
dotnet ef database update --project src/Insurance.Infrastructure --startup-project src/Insurance.Api

# Remover última migration
dotnet ef migrations remove --project src/Insurance.Infrastructure --startup-project src/Insurance.Api
```

### Schema do Banco

```sql
-- Tabela de Segurados
Insureds
├── Id (int, PK)
├── Name (nvarchar(200))
├── Cpf (nvarchar(11))
└── Age (int)

-- Tabela de Veículos
Vehicles
├── Id (int, PK)
├── Model (nvarchar(100))
└── Value (decimal(18,2))

-- Tabela de Apólices
InsurancePolicies
├── Id (int, PK)
├── VehicleId (int, FK)
├── InsuredId (int, FK)
├── RiskRate (decimal(5,2))
├── RiskPremium (decimal(18,2))
├── PurePremium (decimal(18,2))
├── CommercialPremium (decimal(18,2))
├── InsuranceValue (decimal(18,2))
└── CreatedAt (datetime2)
```

## 🐳 Docker

### Construir imagem

```bash
docker build -t vehicle-insurance-api -f src/Insurance.Api/Dockerfile .
```

### Executar com Docker Compose

```bash
# Iniciar serviços
docker-compose up -d

# Ver logs
docker-compose logs -f

# Parar serviços
docker-compose down

# Parar e remover volumes (limpar dados)
docker-compose down -v
```

## 📝 Requisitos Atendidos

✅ **API .NET Core** - Desenvolvida com .NET 8  
✅ **Clean Architecture** - Separação clara de responsabilidades  
✅ **Banco de Dados Relacional** - SQL Server 2022  
✅ **Cálculo de Seguro** - Implementado conforme especificação  
✅ **Persistência de Dados** - Seguro, Veículo e Segurado  
✅ **Pesquisa de Seguros** - Endpoints para buscar por ID e listar todos  
✅ **Relatório JSON** - Endpoint com médias aritméticas  
✅ **Front-End Web** - Dashboard moderno com gráficos  
✅ **Testes Unitários** - Cobertura dos componentes principais  
✅ **Serviço REST Externo** - Mock de serviço de dados do segurado  
✅ **Containerização** - Docker e Docker Compose configurados  

## 🎯 Funcionalidades Extras

- 🔍 **Validação de CPF** - Value Object com validação completa
- 💰 **Value Object Money** - Tipagem forte para valores monetários
- 📊 **Dashboard Moderno** - Interface rica com Chart.js
- 🔄 **Auto-refresh** - Atualização automática do dashboard
- 📈 **Gráficos Interativos** - Visualização de dados em tempo real
- 🐳 **Docker Compose** - Orquestração completa com SQL Server
- 🔒 **FluentValidation** - Validações expressivas e testáveis
- 📝 **Swagger/OpenAPI** - Documentação interativa da API
- ♻️ **Retry Policy** - Resiliência na conexão com banco de dados

## 🏛️ Princípios Aplicados

- **SOLID** - Princípios de design orientado a objetos
- **DDD** - Domain-Driven Design (Value Objects, Entities, Repositories)
- **Clean Architecture** - Separação de responsabilidades
- **Dependency Injection** - Inversão de controle
- **Repository Pattern** - Abstração de acesso a dados
- **DTO Pattern** - Transferência de dados entre camadas

## 📖 Documentação da API

Após iniciar a aplicação, acesse:
- **Swagger UI**: http://localhost:5000/swagger
- **OpenAPI JSON**: http://localhost:5000/swagger/v1/swagger.json

