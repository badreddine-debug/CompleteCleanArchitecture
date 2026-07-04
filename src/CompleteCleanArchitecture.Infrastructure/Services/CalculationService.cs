using CompleteCleanArchitecture.Application.Common.Interfaces;

namespace CompleteCleanArchitecture.Infrastructure.Services;

public class CalculationService : ICalculationService
{
    public decimal Add(decimal left, decimal right)
    {
        return left + right;
    }

    public decimal Subtract(decimal left, decimal right)
    {
        return left - right;
    }

    public decimal Multiply(decimal left, decimal right)
    {
        return left * right;
    }

    public decimal Divide(decimal left, decimal right)
    {
        if (right == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }

        return left / right;
    }

    public decimal Sum(IEnumerable<decimal> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        return values.Sum();
    }

    public decimal Average(IEnumerable<decimal> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        var numbers = values.ToArray();
        if (numbers.Length == 0)
        {
            throw new InvalidOperationException("Cannot calculate the average of an empty collection.");
        }

        return numbers.Average();
    }
}
