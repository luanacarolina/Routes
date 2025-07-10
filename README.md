# Route API

API para cadastro e consulta de rotas aéreas, incluindo cálculo da melhor rota entre dois pontos.

---

## Tecnologias

- ASP.NET Core 8
- Entity Framework Core 9 (SQLite)
- Swagger (OpenAPI) para documentação interativa
- xUnit e Moq para testes unitários

---

## Estrutura do Projeto

- **API/Route.API**: Projeto da API web com controllers e configuração do Swagger.
- **Domain/Route.Domain**: Lógica de negócio, serviços e contratos.
- **Infra/Route.Infra**: Implementação do acesso a dados com EF Core e repositórios.
- **Tests/Route.Tests**: Testes unitários com xUnit e Moq.

---

## Configuração e Execução

1. **Restaurar pacotes e compilar**

```bash
dotnet restore
dotnet build
