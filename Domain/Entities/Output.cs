using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class Output
{
    // Save to DB 
    public int Id { get; set; }
    public int InputId { get; set; }
    public DateTime CalculatedAt { get; set; }

    // Calculated Metrics
    public double BMI { get; set; } // Body Mass Index
    public double BMR { get; set; } // Basal Metabolic Rate
    public double TDEE { get; set; } // Total Daily Energy Expenditure
    public double BFP { get; set; } // Body Fat Percentage
    public double LBM { get; set; } // Lean Body Mass
    public double WtHR { get; set; } // Waist-to-Height Ratio
}
