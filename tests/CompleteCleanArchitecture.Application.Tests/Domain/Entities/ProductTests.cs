using System;
using CompleteCleanArchitecture.Domain.Entities;
using CompleteCleanArchitecture.Domain.Exceptions;
using Xunit;

namespace CompleteCleanArchitecture.Application.Tests.Domain.Entities
{
    public sealed class ProductTests
    {
        [Fact]
        public void Create_WithValidValues_SetsProperties()
        {
            var product = Product.Create("Mon produit", 9.99m, 10);

            Assert.Equal("Mon produit", product.Name);
            Assert.Equal(9.99m, product.Price);
            Assert.Equal(10, product.Stock);
            Assert.True(product.IsActive);
            Assert.NotEqual(default, product.CreatedAtUtc);
            Assert.Null(product.UpdatedAtUtc);
        }

        [Fact]
        public void Create_WithEmptyName_ThrowsDomainException()
        {
            Assert.Throws<DomainException>(() => Product.Create("", 1m, 0));
            Assert.Throws<DomainException>(() => Product.Create("   ", 1m, 0));
        }

        [Fact]
        public void Create_WithNegativePrice_ThrowsDomainException()
        {
            Assert.Throws<DomainException>(() => Product.Create("P", -0.01m, 0));
        }

        [Fact]
        public void Create_WithNegativeStock_ThrowsDomainException()
        {
            Assert.Throws<DomainException>(() => Product.Create("P", 1m, -1));
        }

        [Fact]
        public void Rename_UpdatesNameAndUpdatedAtUtc()
        {
            var product = Product.Create("Old", 1m, 1);
            var before = product.CreatedAtUtc;

            product.Rename("New name");

            Assert.Equal("New name", product.Name);
            Assert.True(product.UpdatedAtUtc.HasValue);
            Assert.True(product.UpdatedAtUtc.Value > before);
        }

        [Fact]
        public void UpdateDescription_TrimsAndAllowsNull()
        {
            var product = Product.Create("P", 1m, 1);

            product.UpdateDescription("  description  ");
            Assert.Equal("description", product.Description);
            Assert.True(product.UpdatedAtUtc.HasValue);

            product.UpdateDescription(null);
            Assert.Equal(string.Empty, product.Description);
        }

        [Fact]
        public void UpdatePrice_Valid_UpdatesAndTouches()
        {
            var product = Product.Create("P", 1m, 1);

            product.UpdatePrice(5.5m);

            Assert.Equal(5.5m, product.Price);
            Assert.True(product.UpdatedAtUtc.HasValue);
        }

        [Fact]
        public void UpdatePrice_Negative_ThrowsDomainException()
        {
            var product = Product.Create("P", 1m, 1);
            Assert.Throws<DomainException>(() => product.UpdatePrice(-1m));
        }

        [Fact]
        public void AdjustStock_IncreaseAndDecrease_Works()
        {
            var product = Product.Create("P", 1m, 5);

            product.AdjustStock(3);
            Assert.Equal(8, product.Stock);

            product.AdjustStock(-2);
            Assert.Equal(6, product.Stock);
            Assert.True(product.UpdatedAtUtc.HasValue);
        }

        [Fact]
        public void AdjustStock_ResultingNegative_ThrowsDomainException()
        {
            var product = Product.Create("P", 1m, 2);
            Assert.Throws<DomainException>(() => product.AdjustStock(-3));
        }

        [Fact]
        public void SetActive_TogglesAndTouches()
        {
            var product = Product.Create("P", 1m, 1);

            product.SetActive(false);
            Assert.False(product.IsActive);
            Assert.True(product.UpdatedAtUtc.HasValue);

            product.SetActive(true);
            Assert.True(product.IsActive);
        }
    }
}