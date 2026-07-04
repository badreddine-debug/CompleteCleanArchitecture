namespace CompleteCleanArchitecture.Application.Common.Interfaces;

public interface ICalculationService
{
    decimal Add(decimal left, decimal right);
    decimal Subtract(decimal left, decimal right);
    decimal Multiply(decimal left, decimal right);
    decimal Divide(decimal left, decimal right);
    decimal Sum(IEnumerable<decimal> values);
    decimal Average(IEnumerable<decimal> values);
}
