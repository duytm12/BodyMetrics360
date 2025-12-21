using Domain.Entities;

namespace Domain.Interfaces;

public interface IInputRepository
{
    Task<Input?> GetLatestByUserIdAsync(Guid userId);
    Task<Input> AddAsync(Input input);
    Task UpdateAsync(Input input);
}
