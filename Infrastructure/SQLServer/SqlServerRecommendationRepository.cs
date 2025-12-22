using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLServer;

public class SqlServerRecommendationRepository(AppDbContext context) : IRecommendationRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Recommendation> AddAsync(Recommendation recommendation)
    {
        _context.Recommendations.Add(recommendation);
        await _context.SaveChangesAsync();
        return recommendation;
    }

    public async Task<Recommendation?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Recommendations
            .FirstOrDefaultAsync(r => r.UserId == userId);
    }

    public async Task<Recommendation> UpsertAsync(Recommendation recommendation)
    {
        var existing = await GetByUserIdAsync(recommendation.UserId);
        
        if (existing != null)
        {
            // Update existing recommendation
            existing.BmiRecommendation = recommendation.BmiRecommendation;
            existing.BmrRecommendation = recommendation.BmrRecommendation;
            existing.TdeeRecommendation = recommendation.TdeeRecommendation;
            existing.BfpRecommendation = recommendation.BfpRecommendation;
            existing.LbmRecommendation = recommendation.LbmRecommendation;
            existing.WtHrRecommendation = recommendation.WtHrRecommendation;
            
            _context.Recommendations.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }
        else
        {
            // Create new recommendation
            _context.Recommendations.Add(recommendation);
            await _context.SaveChangesAsync();
            return recommendation;
        }
    }
}


