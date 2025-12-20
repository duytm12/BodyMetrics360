using Domain.Entities;

namespace Domain.Interfaces;

public interface IInputRepository
{
    Task<Input?> GetLatestAsync();
    Task<Input> AddAsync(Input input);
    Task UpdateAsync(Input input);
}
