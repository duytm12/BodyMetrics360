using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Services;

public class GetBFP : IGetBFP
{
    public Task<double> CalculateBFPAsync(double waist, double neck, double height, double hip, GenderEnum gender)
    {
        if (waist <= 0) throw new ArgumentException("Waist must be greater than zero.");
        if (neck <= 0) throw new ArgumentException("Neck must be greater than zero.");
        if (height <= 0) throw new ArgumentException("Height must be greater than zero.");
        if (hip <= 0) throw new ArgumentException("Hip must be greater than zero.");

        double bfp = gender switch
        {
            GenderEnum.Male => 86.010 * Math.Log10(waist - neck) - 70.041 * Math.Log10(height) + 36.76,
            GenderEnum.Female => 163.205 * Math.Log10(waist + hip - neck) - 97.684 * Math.Log10(height) - 78.387,
            _ => throw new ArgumentException("Invalid gender.")
        };
        return Task.FromResult(bfp);
    }
}
