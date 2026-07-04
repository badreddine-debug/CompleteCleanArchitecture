using CompleteCleanArchitecture.Domain.Common;
using CompleteCleanArchitecture.Domain.Exceptions;

namespace CompleteCleanArchitecture.Domain.Entities;

public sealed class TodoItem : BaseEntity
{
    private TodoItem()
    {
        Title = string.Empty;
    }

    private TodoItem(string title)
    {
        SetTitle(title);
    }

    public string Title { get; private set; } = string.Empty;
    public bool IsCompleted { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }

    public static TodoItem Create(string title) => new(title);

    public void Rename(string title)
    {
        SetTitle(title);
        Touch();
    }

    public void Complete()
    {
        if (IsCompleted)
        {
            return;
        }

        IsCompleted = true;
        CompletedAtUtc = DateTime.UtcNow;
        Touch();
    }

    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Todo item title is required.");
        }

        Title = title.Trim();
    }
}

