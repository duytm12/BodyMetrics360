using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces;

public interface IGetBMI
{
    Task<double> CalculateBMIAsync(double weight, double height);
    Task<double> CalculateBMRAsync(double weight, double height, int age, GenderEnum gender);
    Task<double> CalculateTDEEAsync(double bmr, ActivityLevelEnum activityLevel);
}
