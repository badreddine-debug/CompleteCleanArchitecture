---
applyTo: "**/Application/Features/**/*.cs,**/Application/Services/**/*.cs"
description: "Instructions pour la création de nouveaux services métier en CQRS/MediatR avec Result<T>, dans un projet .NET 8 Clean Architecture"
---

# Création d'un nouveau service métier (.NET 8 – Clean Architecture – CQRS/MediatR)

Ces instructions s'appliquent lorsqu'une nouvelle fonctionnalité métier est demandée
(ex: "crée un service pour gérer X", "ajoute une Command/Query pour Y").

Le projet utilise le pattern **CQRS via MediatR** : pas de service "classique" avec CRUD
regroupé, mais une **Command ou une Query par cas d'usage**, chacune avec son Handler dédié.
Toutes les opérations retournent un **`Result<T>`** (jamais d'exception pour un cas métier attendu).

## 1. Structure des dossiers

```
src/
├── Domain/
│   ├── Entities/
│   └── Interfaces/
│       └── I{Nom}Repository.cs        <- interface de repository (nommage confirmé)
├── Application/
│   ├── Common/
│   │   └── Result.cs                   <- classe Result<T> partagée
│   ├── Features/
│   │   └── {Nom}/
│   │       ├── Commands/
│   │       │   ├── Create{Nom}Command.cs
│   │       │   ├── Create{Nom}CommandHandler.cs
│   │       │   ├── Update{Nom}Command.cs
│   │       │   ├── Update{Nom}CommandHandler.cs
│   │       │   ├── Delete{Nom}Command.cs
│   │       │   └── Delete{Nom}CommandHandler.cs
│   │       ├── Queries/
│   │       │   ├── Get{Nom}ByIdQuery.cs
│   │       │   ├── Get{Nom}ByIdQueryHandler.cs
│   │       │   ├── GetAll{Nom}Query.cs
│   │       │   └── GetAll{Nom}QueryHandler.cs
│   │       └── Validators/
│   │           └── Create{Nom}CommandValidator.cs   <- FluentValidation si utilisé
│   └── DTOs/
│       └── {Nom}Dto.cs
├── Infrastructure/
│   └── Repositories/
│       └── {Nom}Repository.cs
└── API/
    └── Controllers/
        └── {Nom}Controller.cs          <- injecte IMediator, jamais les Handlers directement
```

## 2. Conventions de nommage

- Commands : `Create{Nom}Command`, `Update{Nom}Command`, `Delete{Nom}Command`
- Queries : `Get{Nom}ByIdQuery`, `GetAll{Nom}Query`
- Handlers : suffixe `Handler` (ex: `Create{Nom}CommandHandler`)
- Repository : `I{Nom}Repository` / `{Nom}Repository`
- DTOs : `{Nom}Dto`, `Create{Nom}Dto`, `Update{Nom}Dto`

## 3. Squelette d'une Command + Handler

```csharp
namespace Application.Features.{Nom}.Commands;

public record Create{Nom}Command(Create{Nom}Dto Dto) : IRequest<Result<{Nom}Dto>>;

public class Create{Nom}CommandHandler : IRequestHandler<Create{Nom}Command, Result<{Nom}Dto>>
{
    private readonly I{Nom}Repository _repository;
    private readonly ILogger<Create{Nom}CommandHandler> _logger;

    public Create{Nom}CommandHandler(I{Nom}Repository repository, ILogger<Create{Nom}CommandHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<{Nom}Dto>> Handle(Create{Nom}Command request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = request.Dto.ToEntity();
            await _repository.AddAsync(entity, cancellationToken);

            _logger.LogInformation("{{Nom}} créé avec succès, Id: {Id}", entity.Id);
            return Result<{Nom}Dto>.Success(entity.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la création du {{Nom}}");
            return Result<{Nom}Dto>.Failure("Erreur lors de la création.");
        }
    }
}
```

## 4. Squelette d'une Query + Handler

```csharp
namespace Application.Features.{Nom}.Queries;

public record Get{Nom}ByIdQuery(int Id) : IRequest<Result<{Nom}Dto>>;

public class Get{Nom}ByIdQueryHandler : IRequestHandler<Get{Nom}ByIdQuery, Result<{Nom}Dto>>
{
    private readonly I{Nom}Repository _repository;

    public Get{Nom}ByIdQueryHandler(I{Nom}Repository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<{Nom}Dto>> Handle(Get{Nom}ByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return entity is null
            ? Result<{Nom}Dto>.Failure($"{{Nom}} introuvable pour l'Id {request.Id}.")
            : Result<{Nom}Dto>.Success(entity.ToDto());
    }
}
```

## 5. Rappel du pattern Result<T>

```csharp
namespace Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);
}
```

## 6. Règles obligatoires

- **Une Command/Query = un fichier + un Handler dédié**, jamais de service fourre-tout multi-responsabilités.
- **Aucune exception levée pour un cas métier attendu** (introuvable, validation, règle métier) : retourner `Result<T>.Failure(...)`. Les exceptions restent réservées aux cas techniques imprévus (capturées et loguées, puis converties en `Result.Failure`).
- **Injection de dépendances** via interfaces uniquement (`I{Nom}Repository`, jamais l'implémentation concrète).
- **Async/await** partout où il y a accès BDD/IO, avec `CancellationToken` propagé.
- **Validation des arguments** : `ArgumentNullException.ThrowIfNull(...)` dans les constructeurs.
- **Logging** : `ILogger<T>` injecté dans les Handlers, logs sur succès/échec des opérations sensibles.
- **Mapping** entité ↔ DTO via méthodes d'extension `ToDto()` / `ToEntity()` ou AutoMapper, jamais dispersé.
- **Le Controller** n'a aucune logique métier : il construit la Command/Query et appelle `_mediator.Send(...)`, puis traduit le `Result<T>` en réponse HTTP (`Ok`, `NotFound`, `BadRequest`).

## 7. Enregistrement dans l'injection de dépendances

MediatR se configure une seule fois pour scanner l'assembly (pas de `services.AddScoped` par Handler) :

```csharp
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Create{Nom}Command).Assembly));
```

Le repository, lui, reste enregistré explicitement :

```csharp
services.AddScoped<I{Nom}Repository, {Nom}Repository>();
```

## 8. Points à demander si ambigus

Si la demande ne précise pas l'entité concernée ou les opérations attendues (CRUD complet
vs opérations spécifiques), demander une clarification avant de générer le code plutôt que
de supposer un CRUD complet par défaut.
