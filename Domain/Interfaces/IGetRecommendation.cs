

using Domain.Enums;

namespace Domain.Interfaces;

public interface IGetRecommendation
{

    Task<Dictionary<string, string>> GetBmiRecommendation(double bmi);
    Task<string> GetBmrRecommendation(double bmr);
    Task<Dictionary<string, string>> GetTdeeRecommendation(double tdee);

    Task<Dictionary<string, string>> GetBfpRecommendation(double bfp, GenderEnum gender);

    Task<Dictionary<string, string>> GetLbmRecommendation(double lbm, GenderEnum gender);

    Task<Dictionary<string, string>> GetWthrRecommendation(double wthr);

}

