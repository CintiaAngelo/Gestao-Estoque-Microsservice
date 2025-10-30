# 🏪 Sistema de Gerenciamento de Estoque

Sistema completo para controle de estoque com gestão de produtos perecíveis e não perecíveis, movimentações de entrada/saída e relatórios automatizados.

## 📋 Regras de Negócio Implementadas

### ✅ **Validações de Integridade**
- **Produtos perecíveis** devem ter lote e data de validade
- **Não é permitido** entrada/saída de quantidade negativa
- **Verificação de estoque** suficiente antes de saídas
- **SKU único** para cada produto

### ✅ **Validações de Regras Complexas**
- **Produto perecível não pode** ter movimentação após data de validade
- **Alertas automáticos** quando estoque fica abaixo do mínimo
- **Cálculo automático** do saldo após movimentações
- **Valor total do estoque** calculado em tempo real

### ✅ **Funcionalidades Principais**
- 📦 Cadastro completo de produtos
- 🔄 Movimentações de entrada e saída
- 📊 Relatórios de estoque
- ⚠️ Sistema de alertas
- 🎯 Validações em tempo real

## 🗄️ Diagrama das Entidades

```
┌─────────────────┐    ┌──────────────────────┐
│    PRODUTO      │    │ MOVIMENTACAO_ESTOQUE │
├─────────────────┤    ├──────────────────────┤
│ Id (PK)         │◄───│ Id (PK)              │
│ CodigoSKU (UK)  │    │ Tipo                 │
│ Nome            │    │ Quantidade           │
│ Categoria       │    │ DataMovimentacao     │
│ PrecoUnitario   │    │ Lote                 │
│ QuantidadeMinima│    │ DataValidade         │
│ QuantidadeEstoque│   │ ProdutoId (FK)       │
│ DataCriacao     │    └──────────────────────┘
└─────────────────┘

Categoria: PERECIVEL | NAO_PERECIVEL
TipoMovimentacao: ENTRADA | SAIDA
```

## 🚀 Como Executar o Projeto

### Pré-requisitos
- .NET 6.0 ou superior
- MySQL Server
- Git

### Configuração

1. **Clone o repositório**
```bash
git clone <url-do-repositorio>
cd estoque-api
```

2. **Configure o banco de dados**
```sql
CREATE DATABASE estoque;
USE estoque;

CREATE TABLE Produto (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CodigoSKU VARCHAR(50) NOT NULL UNIQUE,
    Nome VARCHAR(100) NOT NULL,
    Categoria ENUM('PERECIVEL', 'NAO_PERECIVEL') NOT NULL,
    PrecoUnitario DECIMAL(10,2) NOT NULL,
    QuantidadeMinima INT NOT NULL,
    QuantidadeEstoque INT DEFAULT 0,
    DataCriacao DATETIME
);

CREATE TABLE MovimentacaoEstoque (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Tipo ENUM('ENTRADA', 'SAIDA') NOT NULL,
    Quantidade INT NOT NULL,
    DataMovimentacao DATETIME,
    Lote VARCHAR(50),
    DataValidade DATETIME,
    ProdutoId INT NOT NULL,
    FOREIGN KEY (ProdutoId) REFERENCES Produto(Id)
);
```

3. **Configure a connection string** no `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=estoque;User=root;Password=sua_senha;"
  }
}
```

4. **Execute a aplicação**
```bash
dotnet restore
dotnet run
```

5. **Acesse a API**
```
https://localhost:7000/swagger
```

## 📡 Exemplos de Requisições API

### 🏷️ **Produtos**

**Cadastrar Produto Perecível**
```bash
POST /api/produto
{
  "codigoSKU": "LEITE-001",
  "nome": "Leite Integral",
  "categoria": "PERECIVEL",
  "precoUnitario": 6.50,
  "quantidadeMinima": 10
}
```

**Cadastrar Produto Não Perecível**
```bash
POST /api/produto
{
  "codigoSKU": "ARROZ-001",
  "nome": "Arroz Branco",
  "categoria": "NAO_PERECIVEL",
  "precoUnitario": 8.90,
  "quantidadeMinima": 15
}
```

**Listar Produtos Abaixo do Mínimo**
```bash
GET /api/produto/abaixo-minimo
```

### 🔄 **Movimentações**

**Entrada de Estoque (Perecível)**
```bash
POST /api/movimentacao
{
  "tipo": "ENTRADA",
  "quantidade": 50,
  "produtoId": 1,
  "lote": "LOTE-001",
  "dataValidade": "2024-12-31"
}
```

**Saída de Estoque**
```bash
POST /api/movimentacao
{
  "tipo": "SAIDA",
  "quantidade": 15,
  "produtoId": 1
}
```

### 📊 **Relatórios**

**Valor Total do Estoque**
```bash
GET /api/relatorio/valor-total
```

**Produtos Prestes a Vencer (7 dias)**
```bash
GET /api/relatorio/vencendo
```

**Produtos Abaixo do Estoque Mínimo**
```bash
GET /api/relatorio/abaixo-minimo
```

## 🧪 Cenários de Teste Validados

### ❌ **Cenários de Erro (Validações)**
1. **Produto perecível sem data de validade**
2. **Saída com quantidade maior que estoque disponível**
3. **Movimentação com quantidade negativa**
4. **Produto perecível vencido**
5. **SKU duplicado**

### ✅ **Cenários de Sucesso**
1. **Cálculo correto do saldo** após múltiplas movimentações
2. **Alertas automáticos** de estoque mínimo
3. **Validação de datas** para produtos perecíveis
4. **Cálculo do valor total** do estoque

## 🏷️ Comprovantes de Implementação

### ✅ **Commits Realizados**
- `Etapa 1 - Modelagem do domínio` - Estrutura inicial e entidades
- `Etapa 2 - Implementação das regras de negócio` - Serviços e validações
- `Etapa 3 - Validações e tratamento de erros` - Tratamento completo de exceções

### ✅ **Tag da Versão Final**
```bash
git tag -a versao-final -m "Versão final do sistema de estoque"
git push origin versao-final
```

## 🛠️ Tecnologias Utilizadas

- **.NET 6** - Framework principal
- **MySQL** - Banco de dados
- **Dapper** - ORM para consultas
- **Swagger** - Documentação da API
- **Entity Framework** - Mapeamento objeto-relacional

## 📁 Estrutura do Projeto

```
EstoqueAPI/
├── Controllers/          # API Controllers
├── Domain/              # Entidades e Enums
├── Services/            # Lógica de negócio
├── Infrastructure/      # Repositórios e Data Access
├── appsettings.json     # Configurações
└── Program.cs           # Configuração da aplicação
```
