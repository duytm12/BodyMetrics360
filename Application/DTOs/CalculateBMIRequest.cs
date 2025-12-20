using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CalculateBMIRequest
{
    [Required(ErrorMessage = "Weight in pounds is required.")]
    [Range(1, 500, ErrorMessage = "Weight in pounds must be between 1 and 500.")]
    [Display(Name = "Weight (lbs)")]
    public double WeightLbs { get; set; }


    [Required(ErrorMessage = "Height in inches is required.")]
    [Range(1, 100, ErrorMessage = "Height in inches must be between 1 and 100.")]
    [Display(Name = "Height (inches)")]
    public double HeightInches { get; set; }


    [Required(ErrorMessage = "Age is required.")]
    [Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
    [Display(Name = "Age")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Gender is required.")]
    [Display(Name = "Gender")]
    public GenderEnum Gender { get; set; }


    [Required(ErrorMessage = "Activity Level is required.")]
    [Display(Name = "Activity Level")]
    public ActivityLevelEnum ActivityLevel { get; set; }
}
