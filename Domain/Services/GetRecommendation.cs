
using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Services;

public class GetRecommendation: IGetRecommendation
{
    public Task<Dictionary<string, string>> GetBfpRecommendation(double bfp, GenderEnum gender)
    {
        var bfpCategory = (gender, bfp) switch
        {
            (GenderEnum.Male, <= 5 ) => "Essential Fat",
            (GenderEnum.Male, <= 13) => "Athletes",
            (GenderEnum.Male, <= 17) => "Fitness",
            (GenderEnum.Male, <= 24) => "Average",
            (GenderEnum.Male, > 24) => "Obese",

            (GenderEnum.Female, <= 13) => "Essential Fat",
            (GenderEnum.Female, <= 20) => "Athletes",
            (GenderEnum.Female, <= 24) => "Fitness",
            (GenderEnum.Female, <= 31) => "Average",
            (GenderEnum.Female, >31) => "Obese",

            _ => throw new ArgumentOutOfRangeException(nameof(bfp), "Invalid BFP value")
        };

        var bfpRecommendation = bfpCategory switch
        {
            "Essential Fat" => "Maintain your current body fat percentage with a balanced diet and regular exercise.",
            "Athletes" => "Continue your training regimen and focus on performance optimization.",
            "Fitness" => "Incorporate a mix of cardio and strength training to maintain your fitness level.",
            "Average" => "Consider a combination of diet and exercise to reduce body fat percentage.",
            "Obese" => "Consult a healthcare professional for a personalized weight loss plan.",
            _ => throw new ArgumentOutOfRangeException(nameof(bfp), "Invalid BFP category")
        };

        return Task.FromResult(new Dictionary<string, string>
        {
            { "Category", bfpCategory },
            { "Recommendation", bfpRecommendation }
        });
    }

    public Task<Dictionary<string, string>> GetBmiRecommendation(double bmi)
    {
        var bmiCategory = bmi switch
        {
            < 18.5 => "Underweight",
            <24.9 => "Normal weight",
            <29.9 => "Overweight",
            < 34.9 => "Obesity class I",
            < 39.9 => "Obesity class II",
            >= 40 => "Obesity class III",
            _ => throw new ArgumentOutOfRangeException(nameof(bmi), "Invalid BMI value")
        };

        var bmiRecommendation = bmi switch
        {
            < 18.5 => "Consider gaining weight through a balanced diet and strength training.",
            <24.9 => "Maintain your current lifestyle with regular exercise and a healthy diet.",
            <29.9 => "Incorporate more physical activity and monitor your diet to lose weight.",
            < 34.9 => "Consult a healthcare provider for a personalized weight loss plan.",
            < 39.9 => "Seek medical advice for a comprehensive weight management program.",
            >= 40 => "Immediate medical intervention may be necessary; consult a healthcare professional.",
            _ => throw new ArgumentOutOfRangeException(nameof(bmi), "Invalid BMI value")
        };

        return Task.FromResult(new Dictionary<string, string>
        {
            { "Category", bmiCategory },
            { "Recommendation", bmiRecommendation }
        });
    }

    public Task<string> GetBmrRecommendation(double bmr)
    {
        var bmrRecommendation = $"Your body requires {bmr:F2} calories/day to maintain your basic life functions (breathing, circulation, etc.).";
        return Task.FromResult(bmrRecommendation);
    }

    public Task<Dictionary<string, string>> GetLbmRecommendation(double lbm, GenderEnum gender)
    {
        var lbmCategory = (gender, lbm) switch
        {
            (GenderEnum.Male, < 69) => "Below Average",
            (GenderEnum.Male, < 76) => "Average",
            (GenderEnum.Male, < 81) => "Fitness Enthusiasts",
            (GenderEnum.Male, >= 81) => "Athletes",
            (GenderEnum.Female, < 61) => "Below Average",
            (GenderEnum.Female, < 68) => "Average",
            (GenderEnum.Female, < 73) => "Fitness Enthusiasts",
            (GenderEnum.Female, >= 73) => "Athletes",
            _ => throw new ArgumentOutOfRangeException(nameof(lbm), "Invalid LBM value")
        };

        var lbmRecommendation = lbmCategory switch
        {
            "Below Average" => "Focus on strength training and a protein-rich diet to increase lean body mass.",
            "Average" => "Maintain a balanced workout routine with both cardio and strength training.",
            "Fitness Enthusiasts" => "Continue your fitness regimen and consider increasing intensity for further gains.",
            "Athletes" => "Optimize your training and nutrition for peak performance.",
            _ => throw new ArgumentOutOfRangeException(nameof(lbm), "Invalid LBM category")
        };

        return Task.FromResult(new Dictionary<string, string>
        {
            { "Category", lbmCategory },
            { "Recommendation", lbmRecommendation }
        });
    }

    public Task<Dictionary<string, string>> GetTdeeRecommendation(double tdee)
    {
        var tdeeBasic = $"You need to eat {tdee:F2} calories/day to maintain your current weight.";
        var tdeeRecommendation = $"To lose weight, consider a calorie intake of {tdee - 250:F2} calories/day. To gain weight, consider a calorie intake of {tdee + 250:F2} calories/day.";

        return Task.FromResult(new Dictionary<string, string>
        {
            { "Category", tdeeBasic },
            { "Recommendation", tdeeRecommendation }
        });
    }

    public Task<Dictionary<string, string>> GetWthrRecommendation(double wthr)
    {
        var wthrCategory = wthr switch
        {
            <= 0.4 => "Underweight",
            <= 0.49 => "Healthy",
            <= 0.59 => "Overweight",
            > 0.59 => "Obese",
            _ => throw new ArgumentOutOfRangeException(nameof(wthr), "Invalid WtHr value")
        };

        var wthrRecommendation = wthrCategory switch
        {
            "Underweight" => "Consider gaining weight through a balanced diet and strength training.",
            "Healthy" => "Maintain your current lifestyle with regular exercise and a healthy diet.",
            "Overweight" => "Incorporate more physical activity and monitor your diet to lose weight.",
            "Obese" => "Consult a healthcare provider for a personalized weight loss plan.",
            _ => throw new ArgumentOutOfRangeException(nameof(wthr), "Invalid WtHr category")
        };

        return Task.FromResult(new Dictionary<string, string>
        {
            { "Category", wthrCategory },
            { "Recommendation", wthrRecommendation }
        });
    }
}






