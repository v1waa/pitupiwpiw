namespace Lab2.Core;

public sealed class DeliveryCalculator
{
    public decimal CalculateCost(DeliveryRequest request)
    {
        if (request is null)
            throw new ArgumentException("Заявка не должна быть null.", nameof(request));

        if (request.DistanceKm <= 0m || request.DistanceKm > 5000m)
            throw new ArgumentException("Расстояние должно быть больше 0 и не больше 5000 км.", nameof(request.DistanceKm));

        if (request.WeightKg <= 0m || request.WeightKg > 1000m)
            throw new ArgumentException("Вес должен быть больше 0 и не больше 1000 кг.", nameof(request.WeightKg));

        decimal baseTariff;
        if (request.DistanceKm <= 10m)
            baseTariff = 200m;
        else if (request.DistanceKm <= 100m)
            baseTariff = 200m + 20m * (request.DistanceKm - 10m);
        else if (request.DistanceKm <= 1000m)
            baseTariff = 2000m + 15m * (request.DistanceKm - 100m);
        else
            baseTariff = 15500m + 10m * (request.DistanceKm - 1000m);

        decimal weightSurcharge;
        if (request.WeightKg <= 5m)
            weightSurcharge = 0m;
        else if (request.WeightKg <= 20m)
            weightSurcharge = 100m;
        else if (request.WeightKg <= 100m)
            weightSurcharge = 300m;
        else
            weightSurcharge = 500m;

        decimal multiplier = request.Type switch
        {
            DeliveryType.Standard => 1m,
            DeliveryType.Express => 1.5m,
            DeliveryType.Overnight => 2.5m,
            _ => throw new ArgumentException("Неизвестный тип доставки.", nameof(request.Type))
        };

        decimal total = baseTariff * multiplier + weightSurcharge;
        if (request.IsFragile)
            total *= 1.15m;

        return total;
    }
}
