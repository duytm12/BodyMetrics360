using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.SQLServer;

public class SqlServerInputRepository(AppDbContext context) : IInputRepository
{
    private readonly AppDbContext _context = context;
    public async Task<Input> AddAsync(Input input)
    {
        if (input.CreatedAt == default)
        {
            input.CreatedAt = DateTime.UtcNow;
        }
        _context.Inputs.Add(input);
        await _context.SaveChangesAsync();
        return input;
    }

    public async Task<Input?> GetLatestAsync()
    {
        return await _context.Inputs
            .OrderByDescending(i => i.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(Input input)
    {
        _context.Inputs.Update(input);
        await _context.SaveChangesAsync();
    }
}
