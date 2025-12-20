using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs;

public class CalculateLBMRequest
{
    [Required(ErrorMessage = "Weight in pounds is required.")]
    [Range(1, 500, ErrorMessage = "Weight in pounds must be between 1 and 500.")]
    [Display(Name = "Weight (lbs)")]
    public double WeightLbs { get; set; }

    [Required(ErrorMessage = "Height in inches is required.")]
    [Range(1, 100, ErrorMessage = "Height in inches must be between 1 and 100.")]
    [Display(Name = "Height (inches)")]
    public double HeightInches { get; set; }

    [Required(ErrorMessage = "Waist measurement is required.")]
    [Range(1, 200, ErrorMessage = "Waist measurement must be between 1 and 200 inches.")]
    [Display(Name = "Waist (inches)")]
    public double WaistInches { get; set; }

    [Required(ErrorMessage = "Neck measurement is required.")]
    [Range(1, 50, ErrorMessage = "Neck measurement must be between 1 and 50 inches.")]
    [Display(Name = "Neck (inches)")]
    public double NeckInches { get; set; }

    [Required(ErrorMessage = "Hip measurement is required.")]
    [Range(1, 200, ErrorMessage = "Hip measurement must be between 1 and 200 inches.")]
    [Display(Name = "Hip (inches)")]
    public double HipInches { get; set; }


    [Required(ErrorMessage ="Gender is required")]
    [Display(Name = "Gender")]
    public GenderEnum Gender { get; set; }

}
