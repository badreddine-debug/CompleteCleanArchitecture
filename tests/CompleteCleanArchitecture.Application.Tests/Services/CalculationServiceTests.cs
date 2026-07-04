using System;
using System.Collections.Generic;
using CompleteCleanArchitecture.Infrastructure.Services;
using Xunit;

namespace CompleteCleanArchitecture.Application.Tests.Services
{
    public class CalculationServiceTests
    {
        private readonly CalculationService _service = new();

        [Fact]
        public void Add_ReturnsSum()
        {
            var result = _service.Add(1.5m, 2.5m);
            Assert.Equal(4.0m, result);
        }

        [Fact]
        public void Subtract_ReturnsDifference()
        {
            var result = _service.Subtract(5.0m, 3.25m);
            Assert.Equal(1.75m, result);
        }

        [Fact]
        public void Multiply_ReturnsProduct()
        {
            var result = _service.Multiply(2.5m, 4m);
            Assert.Equal(10.0m, result);
        }

        [Fact]
        public void Divide_ReturnsQuotient()
        {
            var result = _service.Divide(10m, 2m);
            Assert.Equal(5m, result);
        }

        [Fact]
        public void Divide_ByZero_ThrowsDivideByZeroException()
        {
            Assert.Throws<DivideByZeroException>(() => _service.Divide(1m, 0m));
        }

        [Fact]
        public void Sum_WithValues_ReturnsSum()
        {
            var values = new decimal[] { 1.0m, 2.5m, 3.5m };
            var result = _service.Sum(values);
            Assert.Equal(7.0m, result);
        }

        [Fact]
        public void Sum_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _service.Sum((IEnumerable<decimal>)null!));
        }

        [Fact]
        public void Average_WithValues_ReturnsAverage()
        {
            var values = new decimal[] { 2.0m, 4.0m, 6.0m };
            var result = _service.Average(values);
            Assert.Equal(4.0m, result);
        }


        [Fact]
        public void Average_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _service.Average((IEnumerable<decimal>)null!));
        }

        [Fact]
        public void Average_Empty_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _service.Average(Array.Empty<decimal>()));
        }
    }
}