using Domain.Interfaces;

namespace Domain.Services;

public class GetWtHR : IGetWtHR
{
    public Task<double> CalculateWtHRAsync(double waist, double height)
    {
        if (waist <= 0) throw new ArgumentException("Waist must be greater than zero.");
        if (height <= 0) throw new ArgumentException("Height must be greater than zero.");

        double wthr = waist / height;

        return Task.FromResult(wthr);
    }
}
