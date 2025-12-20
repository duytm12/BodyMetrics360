using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CalculateBFPRequest
{
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

    [Required(ErrorMessage = "Height measurement is required.")]
    [Range(1, 100, ErrorMessage = "Height measurement must be between 1 and 100 inches.")]
    [Display(Name = "Height (inches)")]
    public double HeightInches { get; set; }

    [Required(ErrorMessage = "Gender is required.")]
    [Display(Name = "Gender")]
    public GenderEnum Gender { get; set; }
}
