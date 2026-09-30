using FluentAssertions;
using Lab2.Core;

namespace Lab2.Tests;

public class InsuranceTests
{
    [Theory]
    [InlineData(false, -100, 200)]
    [InlineData(false, 0, 200)]
    [InlineData(false, 10000, 200)]
    [InlineData(true, -100, 200)]
    [InlineData(true, 0, 200)]
    [InlineData(true, 0.01, 300)]
    [InlineData(true, 4999.99, 300)]
    [InlineData(true, 5000, 300)]
    [InlineData(true, 5000.01, 300.0002)]
    [InlineData(true, 10000, 400)]
    public void CalculateCost_WithInsuranceConditions_ReturnsExpectedCost(
        bool isInsured, double declaredValue, double expected)
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(
            isInsured: isInsured, declaredValue: (decimal)declaredValue);

        // ===== ACT =====
        decimal result = calculator.CalculateCost(request);

        // ===== ASSERT =====
        result.Should().Be((decimal)expected, "страховка требует флаг и положительную стоимость");
    }

    [Fact]
    public void CalculateCost_WithAllSurcharges_AddsInsuranceAfterFragileSurcharge()
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(
            50m, 20m, DeliveryType.Express, true, true, 10000m);

        // ===== ACT =====
        decimal result = calculator.CalculateCost(request);

        // ===== ASSERT =====
        result.Should().Be(2040m, "(1000 × 1,5 + 100) × 1,15 + 10000 × 0,02 = 2040");
    }

    [Theory]
    [InlineData(false, false, 200)]
    [InlineData(true, false, 230)]
    [InlineData(false, true, 400)]
    [InlineData(true, true, 430)]
    public void CalculateCost_WithFragileAndInsuranceFlagCombinations_ReturnsExpectedCost(
        bool isFragile, bool isInsured, int expected)
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(
            isFragile: isFragile, isInsured: isInsured, declaredValue: 10000m);

        // ===== ACT =====
        decimal result = calculator.CalculateCost(request);

        // ===== ASSERT =====
        result.Should().Be(expected);
    }
}
