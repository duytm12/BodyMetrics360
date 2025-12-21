using Domain.Entities;

namespace Domain.Interfaces;

public interface IOutputRepository
{
    Task<Output?> GetLatestByUserIdAsync(Guid userId);
    Task<Output?> GetByInputIdAsync(int inputId);
    Task<Output> AddAsync(Output output);
    Task UpdateAsync(Output output);
}
