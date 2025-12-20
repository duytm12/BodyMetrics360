using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces;

public interface IGetWtHR
{
    Task<double> CalculateWtHRAsync(double waist, double height);
}
