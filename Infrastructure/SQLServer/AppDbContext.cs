using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLServer;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Input> Inputs { get; set; }
    public DbSet<Output> Outputs { get; set; }
}
