using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs;

public class CalculateBMIResponse
{
    public double BMI { get; set; }
    public double BMR { get; set; }
    public double TDEE { get; set; }
}
