using CompleteCleanArchitecture.Domain.Common;
using CompleteCleanArchitecture.Domain.Exceptions;

namespace CompleteCleanArchitecture.Domain.Entities;

public sealed class Product : BaseEntity
{
    private Product()
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    private Product(string name, decimal price, int stock)
    {
        SetName(name);
        SetPrice(price);
        SetStock(stock);
        Description = string.Empty;
        IsActive = true;
    }

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public bool IsActive { get; private set; }

    public static Product Create(string name, decimal price, int stock) => new(name, price, stock);

    public void Rename(string name)
    {
        SetName(name);
        Touch();
    }

    public void UpdateDescription(string description)
    {
        Description = description?.Trim() ?? string.Empty;
        Touch();
    }

    public void UpdatePrice(decimal price)
    {
        SetPrice(price);
        Touch();
    }

    public void AdjustStock(int quantity)
    {
        var newStock = Stock + quantity;
        if (newStock < 0)
        {
            throw new DomainException("Stock cannot be negative.");
        }

        Stock = newStock;
        Touch();
    }

    public void SetActive(bool active)
    {
        IsActive = active;
        Touch();
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Product name is required.");
        }

        Name = name.Trim();
    }

    private void SetPrice(decimal price)
    {
        if (price < 0)
        {
            throw new DomainException("Price must be greater than or equal to zero.");
        }

        Price = price;
    }

    private void SetStock(int stock)
    {
        if (stock < 0)
        {
            throw new DomainException("Stock must be greater than or equal to zero.");
        }

        Stock = stock;
    }
}