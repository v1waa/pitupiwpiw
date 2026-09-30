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

        if (!Enum.IsDefined(request.Type))
            throw new ArgumentException("Неизвестный тип доставки.", nameof(request.Type));

        return 200m;
    }
}
