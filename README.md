# VanSpeed Logistics: Sistema de Gestão de Frota (SGF)

Este projeto é uma aplicação web desenvolvida em ASP.NET Core MVC com
Identity e MySQL para gerenciar e monitorar o fluxo de entregas, coletas
e retornos de uma frota logística. O sistema implementa um rigoroso
controle de acesso baseado em Roles para segregar as responsabilidades
entre a equipe de campo (Operators) e a administração (Managers).

## Arquitetura e Tecnologia

| Componente       | Tecnologia / Framework                       |
|------------------|-----------------------------------------------|
| **Backend**      | ASP.NET Core 9.0 (MVC)                        |
| **Autenticação** | ASP.NET Identity (Customizado com ApplicationUser) |
| **Banco de Dados** | MySQL                                      |
| **ORM**          | Entity Framework Core                         |
| **Frontend**     | Razor Pages, HTML, Tailwind CSS               |
| **Segurança**    | Autorização Baseada em Roles                  |



## Fluxos de Usuário e Segurança (Roles)

O sistema opera com dois perfis de usuário definidos em **AspNetRoles**:

### 1. Manager (Gestor de Frota)

**Usuário de Exemplo:** Peter Parker (peter@vanspeed.com)

**Permissões:** - Acesso exclusivo à área administrativa. - Cadastro de
novos Operators. - Visualização futura de dados completos da frota.

### 2. Operator (Motorista)

**Usuário de Exemplo:** Diego Alves (by.doliveira@gmail.com)

**Permissões:** - Acesso ao Dashboard do Operador. - Lançamento de
Entregas, Coletas e Retornos. - Visualização do próprio histórico de
lançamentos.

## Funcionalidades Implementadas

### Segurança e Autenticação

-   Login/Logout com Identity.
-   Controle de acesso via Roles.
-   Confirmação de email obrigatória.

### Dashboard do Operator

-   Cadastro diário de registros.
-   Histórico pessoal de lançamentos.

### Área Administrativa do Manager

-   Cadastro de novos Operators.

## Roadmap (Próximas Funcionalidades)

-   Dashboard Gerencial com KPIs.
-   Relatório completo da frota.
-   Histórico detalhado por motorista.
-   CRUD completo de Operators.
-   Cadastro e gestão de veículos.
-   Associação Motorista--Veículo.

## Configuração Inicial do Projeto

### Pré-requisitos

-   .NET Core SDK 9.0+
-   Servidor MySQL

### 1. Criar Banco de Dados

Atualize a connection string no `appsettings.json`:

    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Database=vanspeed_logistics;Uid=seu_usuario;Pwd=sua_senha;"
    }

### 2. Aplicar Migrations

    dotnet ef database update

### 3. Seed Inicial

O `DbInitializer` cria automaticamente: - Roles Manager e Operator -
Usuário Manager padrão

### 4. Executar a Aplicação

    dotnet run

Acesse via `https://localhost:XXXX`.
