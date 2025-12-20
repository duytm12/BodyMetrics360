using Domain.Enums;

namespace Domain.Interfaces;

public interface IGetBFP
{
    Task<double> CalculateBFPAsync(double waist, double neck, double height, double hip, GenderEnum gender);
}
