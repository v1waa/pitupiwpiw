namespace Lab2.Core;

public sealed class DeliveryRequest
{
    public decimal DistanceKm { get; init; }
    public decimal WeightKg { get; init; }
    public DeliveryType Type { get; init; }
    public bool IsFragile { get; init; }
    public bool IsInsured { get; init; }
    public decimal DeclaredValue { get; init; }
}
