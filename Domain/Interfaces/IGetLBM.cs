using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces;

public interface IGetLBM
{
    Task<double> CalculateLBMAsync(double weight, double height, double neck, double waist, double hip, GenderEnum gender);
}
