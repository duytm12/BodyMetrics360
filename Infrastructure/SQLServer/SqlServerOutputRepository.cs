using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.SQLServer;

public class SqlServerOutputRepository(AppDbContext context) : IOutputRepository
{
    private readonly AppDbContext _context = context;
    public async Task<Output> AddAsync(Output output)
    {
        if (output.CalculatedAt == default)
        {
            output.CalculatedAt = DateTime.UtcNow;
        }
        _context.Outputs.Add(output);
        await _context.SaveChangesAsync();
        return output;
    }

    public async Task<Output?> GetByInputIdAsync(int inputId)
    {
        return await _context.Outputs
            .FirstOrDefaultAsync(o => o.InputId == inputId);
    }

    public async Task<Output?> GetLatestByUserIdAsync(Guid userId)
    {
        return await _context.Outputs
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CalculatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(Output output)
    {
        _context.Outputs.Update(output);
        await _context.SaveChangesAsync();
    }
}
