using Domain.Entities;

namespace Domain.Interfaces;

public interface IRecommendationRepository
{
    Task<Recommendation> AddAsync(Recommendation recommendation);

    Task<Recommendation?> GetByUserIdAsync(Guid userId);

    Task<Recommendation> UpsertAsync(Recommendation recommendation);
}


