using Domain.Enums;

namespace Domain.Interfaces;

public interface IGetLBM
{
    Task<double> CalculateLBMAsync(double weight, double height, double neck, double waist, double hip, GenderEnum gender);
}
