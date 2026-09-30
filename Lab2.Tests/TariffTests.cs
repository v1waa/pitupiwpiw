using FluentAssertions;
using Lab2.Core;

namespace Lab2.Tests;

public class TariffTests
{
    [Theory]
    [InlineData(0.01, 200)]
    [InlineData(9.99, 200)]
    [InlineData(10, 200)]
    [InlineData(10.01, 200.20)]
    [InlineData(50, 1000)]
    [InlineData(99.99, 1999.80)]
    [InlineData(100, 2000)]
    [InlineData(100.01, 2000.15)]
    [InlineData(500, 8000)]
    [InlineData(999.99, 15499.85)]
    [InlineData(1000, 15500)]
    [InlineData(1000.01, 15500.10)]
    [InlineData(2000, 25500)]
    public void CalculateCost_WithDistanceTariffBoundaries_ReturnsExpectedCost(
        double distance, double expected)
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(distanceKm: (decimal)distance);

        // ===== ACT =====
        decimal result = calculator.CalculateCost(request);

        // ===== ASSERT =====
        result.Should().Be((decimal)expected, "на границах должен применяться правильный тариф");
    }

    [Theory]
    [InlineData(4.99, 200)]
    [InlineData(5, 200)]
    [InlineData(5.01, 300)]
    [InlineData(19.99, 300)]
    [InlineData(20, 300)]
    [InlineData(20.01, 500)]
    [InlineData(99.99, 500)]
    [InlineData(100, 500)]
    [InlineData(100.01, 700)]
    [InlineData(1000, 700)]
    public void CalculateCost_WithWeightSurchargeBoundaries_ReturnsExpectedCost(
        double weight, double expected)
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(weightKg: (decimal)weight);

        // ===== ACT =====
        decimal result = calculator.CalculateCost(request);

        // ===== ASSERT =====
        result.Should().Be((decimal)expected);
    }

    [Theory]
    [InlineData(DeliveryType.Standard, 1000)]
    [InlineData(DeliveryType.Express, 1500)]
    [InlineData(DeliveryType.Overnight, 2500)]
    public void CalculateCost_WithDeliveryType_ReturnsMultipliedBaseTariff(
        DeliveryType type, int expected)
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(distanceKm: 50m, type: type);

        // ===== ACT =====
        decimal result = calculator.CalculateCost(request);

        // ===== ASSERT =====
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(DeliveryType.Standard, 1100)]
    [InlineData(DeliveryType.Express, 1600)]
    [InlineData(DeliveryType.Overnight, 2600)]
    public void CalculateCost_WithWeightAndDeliveryType_MultipliesOnlyBaseTariff(
        DeliveryType type, int expected)
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(50m, 20m, type);

        // ===== ACT =====
        decimal result = calculator.CalculateCost(request);

        // ===== ASSERT =====
        result.Should().Be(expected, "коэффициент доставки не умножает надбавку за вес");
    }

    [Theory]
    [InlineData(DeliveryType.Standard, false, 1100)]
    [InlineData(DeliveryType.Standard, true, 1265)]
    [InlineData(DeliveryType.Express, false, 1600)]
    [InlineData(DeliveryType.Express, true, 1840)]
    [InlineData(DeliveryType.Overnight, false, 2600)]
    [InlineData(DeliveryType.Overnight, true, 2990)]
    public void CalculateCost_WithFragileFlag_AppliesSurchargeAfterTypeAndWeight(
        DeliveryType type, bool isFragile, int expected)
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(50m, 20m, type, isFragile);

        // ===== ACT =====
        decimal result = calculator.CalculateCost(request);

        // ===== ASSERT =====
        result.Should().Be(expected);
    }
}
