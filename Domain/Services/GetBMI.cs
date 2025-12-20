using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Services;

public class GetBMI : IGetBMI
{
    public Task<double> CalculateBMIAsync(double weight, double height)
    {
        if (height <= 0) throw new ArgumentException("Height must be greater than zero.");
        if (weight <= 0) throw new ArgumentException("Weight must be greater than zero.");
        return Task.FromResult(weight / (height * height) * 703);

    }

    public Task<double> CalculateBMRAsync(double weight, double height, int age, GenderEnum gender)
    {
        if (weight <= 0) throw new ArgumentException("Weight must be greater than zero.");
        if (height <= 0) throw new ArgumentException("Height must be greater than zero.");
        if (age <= 0) throw new ArgumentException("Age must be greater than zero.");

        double bmr = gender switch
        {
            GenderEnum.Male => (4.536 * weight) + (15.88 * height) - (5 * age) + 5,
            GenderEnum.Female => (4.536 * weight) + (15.88 * height) - (5 * age) - 161,
            _ => throw new ArgumentException("Invalid gender."),
        };

        return Task.FromResult(bmr);
    }

    public Task<double> CalculateTDEEAsync(double bmr, ActivityLevelEnum activityLevel)
    {
        if (bmr <= 0) throw new ArgumentException("BMR must be greater than zero.");
        double activityMultiplier = activityLevel switch
        {
            ActivityLevelEnum.Sedentary => 1.2,
            ActivityLevelEnum.LightlyActive => 1.375,
            ActivityLevelEnum.ModeratelyActive => 1.55,
            ActivityLevelEnum.VeryActive => 1.725,
            ActivityLevelEnum.ExtraActive => 1.9,
            _ => throw new ArgumentException("Invalid activity level."),
        };

        var tdee = bmr * activityMultiplier;
        return Task.FromResult(tdee);
    }
}
