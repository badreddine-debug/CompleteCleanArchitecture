# Complete Clean Architecture - .NET 8

Projet exemple en .NET 8 avec Clean Architecture, CQRS via MediatR, Entity Framework Core et xUnit.

## Structure

```text
src/
  CompleteCleanArchitecture.Api             API ASP.NET Core Minimal API
  CompleteCleanArchitecture.Application     Cas d'usage CQRS, validation, abstractions
  CompleteCleanArchitecture.Domain          Entites et regles metier
  CompleteCleanArchitecture.Infrastructure  EF Core, repositories, persistence
tests/
  CompleteCleanArchitecture.Application.Tests
```

## Lancer le projet

```powershell
dotnet restore
dotnet build
dotnet test
dotnet run --project src/CompleteCleanArchitecture.Api
```

Swagger est disponible en environnement Development.

## Endpoints

- `GET /api/todo-items`
- `POST /api/todo-items` avec body `{ "title": "Ma tache" }`
- `PUT /api/todo-items/{id}/complete`

La base SQLite est creee automatiquement au demarrage via `EnsureCreatedAsync` pour faciliter l'execution locale.
