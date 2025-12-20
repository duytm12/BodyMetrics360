using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class Input
{
    // Save to DB 
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Input data to calculate Metrics
    public double WeightLbs { get; set; }
    public double HeightInches { get; set; }
    public double WaistInches { get; set; }
    public double NeckInches { get; set; }
    public double HipInches { get; set; }
    public int Age { get; set; }
    public GenderEnum Gender { get; set; }
    public ActivityLevelEnum ActivityLevel { get; set; }

    public double ActivityMultiplier => ActivityLevel switch
    {
        ActivityLevelEnum.Sedentary => 1.2,
        ActivityLevelEnum.LightlyActive => 1.375,
        ActivityLevelEnum.ModeratelyActive => 1.55,
        ActivityLevelEnum.VeryActive => 1.725,
        ActivityLevelEnum.ExtraActive => 1.9,
        _ => throw new ArgumentException("Invalid activity level"),
    };  
}
