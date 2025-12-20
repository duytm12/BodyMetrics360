using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces;

public interface IGetBFP
{
    Task<double> CalculateBFPAsync(double waist, double neck, double height, double hip, GenderEnum gender);
}
