using Lab2.Core;

namespace Lab2.Tests;

internal static class TestRequests
{
    internal static DeliveryRequest Create(
        decimal distanceKm = 5m,
        decimal weightKg = 1m,
        DeliveryType type = DeliveryType.Standard,
        bool isFragile = false,
        bool isInsured = false,
        decimal declaredValue = 0m)
    {
        return new DeliveryRequest
        {
            DistanceKm = distanceKm,
            WeightKg = weightKg,
            Type = type,
            IsFragile = isFragile,
            IsInsured = isInsured,
            DeclaredValue = declaredValue
        };
    }
}
