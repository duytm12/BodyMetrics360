using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CalculateWtHRRequest
{
    [Required(ErrorMessage = "Waist measurement is required.")]
    [Range(1, 200, ErrorMessage = "Waist measurement must be between 1 and 200 inches.")]
    [Display(Name = "Waist (inches)")]
    public double WaistInches { get; set; }

    [Required(ErrorMessage = "Height measurement is required.")]
    [Range(1, 100, ErrorMessage = "Height measurement must be between 1 and 100 inches.")]
    [Display(Name = "Height (inches)")]
    public double HeightInches { get; set; }

    [Required(ErrorMessage = "Gender is required.")]
    [Display(Name = "Gender")]
    public GenderEnum Gender { get; set; }
}
