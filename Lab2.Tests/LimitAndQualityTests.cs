using FluentAssertions;
using Lab2.Core;

namespace Lab2.Tests;

public class LimitAndQualityTests
{
    [Theory]
    [InlineData(2489999.50, 49999.99)]
    [InlineData(2490000, 50000)]
    [InlineData(2490000.50, 50000)]
    public void CalculateCost_AroundMaximumCost_ReturnsAtMost50000(
        double declaredValue, double expected)
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(
            distanceKm: 10m, isInsured: true, declaredValue: (decimal)declaredValue);

        // ===== ACT =====
        decimal result = calculator.CalculateCost(request);

        // ===== ASSERT =====
        result.Should().Be((decimal)expected);
    }

    [Theory]
    [InlineData(DeliveryType.Standard, false, false, 1)]
    [InlineData(DeliveryType.Express, false, false, 1000)]
    [InlineData(DeliveryType.Overnight, true, true, 1000)]
    public void CalculateCost_WithMaximumDistance_Returns50000(
        DeliveryType type, bool isFragile, bool isInsured, int weight)
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(5000m, weight, type, isFragile, isInsured, 10000m);

        // ===== ACT =====
        decimal result = calculator.CalculateCost(request);

        // ===== ASSERT =====
        result.Should().Be(50000m);
    }

    [Theory]
    [InlineData(2000, DeliveryType.Overnight, false)]
    [InlineData(4000, DeliveryType.Standard, true)]
    public void CalculateCost_WhenTypeOrFragilityExceedsLimit_Returns50000(
        int distance, DeliveryType type, bool isFragile)
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(distanceKm: distance, type: type, isFragile: isFragile);

        // ===== ACT =====
        decimal result = calculator.CalculateCost(request);

        // ===== ASSERT =====
        result.Should().Be(50000m, "ограничение применяется после всех надбавок");
    }

    [Fact]
    public void CalculateCost_WithLargestDeclaredValue_Returns50000WithoutOverflow()
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(isInsured: true, declaredValue: decimal.MaxValue);

        // ===== ACT =====
        decimal result = calculator.CalculateCost(request);

        // ===== ASSERT =====
        result.Should().Be(50000m);
    }

    [Theory]
    [InlineData(10.001, 200.020)]
    [InlineData(100.001, 2000.015)]
    [InlineData(1000.001, 15500.010)]
    public void CalculateCost_WithFractionalDistance_PreservesDecimalPrecision(
        double distance, double expected)
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(distanceKm: (decimal)distance);

        // ===== ACT =====
        decimal result = calculator.CalculateCost(request);

        // ===== ASSERT =====
        result.Should().Be((decimal)expected, "методичка не требует округления стоимости");
    }

    [Fact]
    public void CalculateCost_WithRepeatedRequest_ReturnsSameCostAndPreservesInput()
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(50m, 20m, DeliveryType.Express, true, true, 10000m);
        var original = new
        {
            request.DistanceKm, request.WeightKg, request.Type,
            request.IsFragile, request.IsInsured, request.DeclaredValue
        };

        // ===== ACT =====
        decimal first = calculator.CalculateCost(request);
        decimal second = calculator.CalculateCost(request);

        // ===== ASSERT =====
        first.Should().Be(2040m);
        second.Should().Be(first);
        request.Should().BeEquivalentTo(original);
    }
}
