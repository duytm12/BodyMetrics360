namespace Domain.Entities;

public class Recommendation
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string BmiRecommendation { get; set; } = string.Empty;
    public string BmrRecommendation { get; set; } = string.Empty;
    public string TdeeRecommendation { get; set; } = string.Empty;
    public string BfpRecommendation { get; set; } = string.Empty;
    public string LbmRecommendation { get; set; } = string.Empty;
    public string WtHrRecommendation { get; set; } = string.Empty;
}