using Domain.Enums;

namespace Domain.Interfaces;

public interface IGetBMI
{
    Task<double> CalculateBMIAsync(double weight, double height);
    Task<double> CalculateBMRAsync(double weight, double height, int age, GenderEnum gender);
    Task<double> CalculateTDEEAsync(double bmr, ActivityLevelEnum activityLevel);
}
