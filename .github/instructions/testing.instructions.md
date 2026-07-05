---
applyTo: "**/*Tests.cs,**/*.Tests/**"
description: Conventions pour les tests unitaires .NET 8
---

# Conventions de tests unitaires — Projet .NET 8

## Stack de test
- Framework : xUnit
- Mocking : Moq
- Assertions : FluentAssertions
- Cible : .NET 8, C# 12

## Structure d'un test
- Pattern AAA obligatoire : `// Arrange`, `// Act`, `// Assert`
- Un seul concept testé par méthode de test
- Pas de logique conditionnelle (if/for) dans un test

## Nommage
- Convention : `MethodeTestee_Scenario_ResultatAttendu`
  - Exemple : `CalculerTotal_QuantiteNegative_LeveArgumentException`
- Classe de test : `NomDeLaClasse` + `Tests` (ex: `OrderServiceTests`)
- Namespace miroir : `MonProjet.Tests.Services`

## Organisation des projets
- Un projet de tests par projet source : `MonProjet.Services.Tests`
- Un fichier de test par classe testée

## Mocking et isolation
- Toute dépendance externe (base de données, HTTP, fichiers) doit être mockée avec Moq
- Utiliser `Mock<IMonInterface>` et vérifier les interactions avec `Verify()` quand pertinent
- Les tests ne doivent jamais dépendre d'un état partagé entre eux

## Tests asynchrones
- Les méthodes `async Task<T>` sont testées avec `async Task` (jamais `async void`)
- Utiliser `await` correctement, ne jamais bloquer avec `.Result` ou `.Wait()`

## Données de test
- Utiliser `[Theory]` + `[InlineData]` pour les cas paramétrés
- Utiliser `[Fact]` pour les cas uniques

## Couverture attendue
- Cas nominal (happy path)
- Cas limites (valeurs nulles, vides, extrêmes)
- Cas d'erreur (exceptions attendues via `Assert.Throws<T>` ou `.Should().Throw<T>()`)

## Exemple de référence
```csharp
public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _repositoryMock = new();
    private readonly OrderService _sut;

    public OrderServiceTests()
    {
        _sut = new OrderService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetOrderById_OrderExists_ReturnsOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(orderId))
            .ReturnsAsync(new Order { Id = orderId });

        // Act
        var result = await _sut.GetOrderByIdAsync(orderId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(orderId);
    }
}
```