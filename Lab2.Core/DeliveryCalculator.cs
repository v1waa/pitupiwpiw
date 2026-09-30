namespace Lab2.Core;

public sealed class DeliveryCalculator
{
    public decimal CalculateCost(DeliveryRequest request)
    {
        ValidateRequest(request);

        decimal total = GetBaseTariff(request.DistanceKm)
            * GetDeliveryMultiplier(request.Type)
            + GetWeightSurcharge(request.WeightKg);

        if (request.IsFragile)
            total *= 1.15m;

        if (request.IsInsured && request.DeclaredValue > 0m)
            total += Math.Max(100m, request.DeclaredValue * 0.02m);

        return Math.Min(50000m, total);
    }

    private static void ValidateRequest(DeliveryRequest request)
    {
        if (request is null)
            throw new ArgumentException("Заявка не должна быть null.", nameof(request));

        if (request.DistanceKm <= 0m || request.DistanceKm > 5000m)
            throw new ArgumentException(
                "Расстояние должно быть больше 0 и не больше 5000 км.",
                nameof(request.DistanceKm));

        if (request.WeightKg <= 0m || request.WeightKg > 1000m)
            throw new ArgumentException(
                "Вес должен быть больше 0 и не больше 1000 кг.",
                nameof(request.WeightKg));
    }

    private static decimal GetBaseTariff(decimal distanceKm)
    {
        if (distanceKm <= 10m)
            return 200m;
        if (distanceKm <= 100m)
            return 200m + 20m * (distanceKm - 10m);
        if (distanceKm <= 1000m)
            return 2000m + 15m * (distanceKm - 100m);
        return 15500m + 10m * (distanceKm - 1000m);
    }

    private static decimal GetWeightSurcharge(decimal weightKg)
    {
        if (weightKg <= 5m)
            return 0m;
        if (weightKg <= 20m)
            return 100m;
        if (weightKg <= 100m)
            return 300m;
        return 500m;
    }

    private static decimal GetDeliveryMultiplier(DeliveryType type)
    {
        return type switch
        {
            DeliveryType.Standard => 1m,
            DeliveryType.Express => 1.5m,
            DeliveryType.Overnight => 2.5m,
            _ => throw new ArgumentException(
                "Неизвестный тип доставки.", nameof(DeliveryRequest.Type))
        };
    }
}
