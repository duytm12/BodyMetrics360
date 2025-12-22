namespace Application.DTOs;

public class MetricsSummary
{
    public double? BMI { get; set; }
    public double? BMR { get; set; }
    public double? TDEE { get; set; }
    public double? LBM { get; set; }
    public double? WtHR { get; set; }
    public double? BFP { get; set; }

    // Recommendations for each metric
    public MetricRecommendation? BmiRecommendation { get; set; }
    public MetricRecommendation? BmrRecommendation { get; set; }
    public MetricRecommendation? TdeeRecommendation { get; set; }
    public MetricRecommendation? BfpRecommendation { get; set; }
    public MetricRecommendation? LbmRecommendation { get; set; }
    public MetricRecommendation? WtHrRecommendation { get; set; }
}
