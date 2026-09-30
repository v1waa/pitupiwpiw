using FluentAssertions;
using Lab2.Core;

namespace Lab2.Tests;

public class ValidationTests
{
    [Fact]
    public void CalculateCost_WithNullRequest_ThrowsArgumentException()
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();

        // ===== ACT =====
        Action act = () => calculator.CalculateCost(null!);

        // ===== ASSERT =====
        act.Should().Throw<ArgumentException>().WithParameterName("request");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(5000.01)]
    public void CalculateCost_WithInvalidDistance_ThrowsArgumentException(double distance)
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(distanceKm: (decimal)distance);

        // ===== ACT =====
        Action act = () => calculator.CalculateCost(request);

        // ===== ASSERT =====
        act.Should().Throw<ArgumentException>()
            .WithParameterName(nameof(DeliveryRequest.DistanceKm));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1000.01)]
    public void CalculateCost_WithInvalidWeight_ThrowsArgumentException(double weight)
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(weightKg: (decimal)weight);

        // ===== ACT =====
        Action act = () => calculator.CalculateCost(request);

        // ===== ASSERT =====
        act.Should().Throw<ArgumentException>()
            .WithParameterName(nameof(DeliveryRequest.WeightKg));
    }

    [Theory]
    [InlineData(0.01, 0.01)]
    [InlineData(5000, 0.01)]
    [InlineData(0.01, 1000)]
    [InlineData(5000, 1000)]
    public void CalculateCost_WithValidRangeBoundaries_ReturnsPositiveCost(
        double distance, double weight)
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create((decimal)distance, (decimal)weight);

        // ===== ACT =====
        decimal result = calculator.CalculateCost(request);

        // ===== ASSERT =====
        result.Should().BeGreaterThan(0m);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3)]
    public void CalculateCost_WithUndefinedDeliveryType_ThrowsArgumentException(int type)
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(type: (DeliveryType)type);

        // ===== ACT =====
        Action act = () => calculator.CalculateCost(request);

        // ===== ASSERT =====
        act.Should().Throw<ArgumentException>()
            .WithParameterName(nameof(DeliveryRequest.Type));
    }

    [Fact]
    public void CalculateCost_WithMinimalStandardDelivery_Returns200()
    {
        // ===== ARRANGE =====
        var calculator = new DeliveryCalculator();
        var request = TestRequests.Create(0.01m, 0.01m);

        // ===== ACT =====
        decimal result = calculator.CalculateCost(request);

        // ===== ASSERT =====
        result.Should().Be(200m, "обычная доставка до 10 км и до 5 кг стоит 200 рублей");
    }
}
