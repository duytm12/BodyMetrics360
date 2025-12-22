using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLServer;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Input> Inputs { get; set; }
    public DbSet<Output> Outputs { get; set; }
    public DbSet<Recommendation> Recommendations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureInputEntity(modelBuilder);
        ConfigureOutputEntity(modelBuilder);
        ConfigureRecommendationEntity(modelBuilder);
    }

    private void ConfigureInputEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Input>(entity =>
        {
            entity.ToTable("UserInput");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.UserId).HasColumnType("uniqueidentifier").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime2").IsRequired().HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.WeightLbs).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.HeightInches).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.WaistInches).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.NeckInches).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.HipInches).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.Age).IsRequired();
            entity.Property(e => e.Gender).HasConversion<int>().IsRequired();
            entity.Property(e => e.ActivityLevel).HasConversion<int>().IsRequired();
            entity.HasIndex(e => e.UserId).HasDatabaseName("IX_UserInput_UserId");
            entity.HasIndex(e => e.CreatedAt).HasDatabaseName("IX_UserInput_CreatedAt");
            entity.Ignore(e => e.ActivityMultiplier);
        });
    }

    private void ConfigureOutputEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Output>(entity =>
        {
            entity.ToTable("UserOutput");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasOne(o => o.Input).WithMany(i => i.Outputs).HasForeignKey(o => o.InputId).OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.InputId).IsRequired();
            entity.Property(e => e.UserId).HasColumnType("uniqueidentifier").IsRequired();
            entity.Property(e => e.CalculatedAt).HasColumnType("datetime2").IsRequired().HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.BMI).HasColumnType("decimal(18,4)").IsRequired();
            entity.Property(e => e.BMR).HasColumnType("decimal(18,4)").IsRequired();
            entity.Property(e => e.TDEE).HasColumnType("decimal(18,4)").IsRequired();
            entity.Property(e => e.BFP).HasColumnType("decimal(18,4)").IsRequired();
            entity.Property(e => e.LBM).HasColumnType("decimal(18,4)").IsRequired();
            entity.Property(e => e.WtHR).HasColumnType("decimal(18,4)").IsRequired();
            entity.HasIndex(e => e.InputId).HasDatabaseName("IX_UserOutput_InputId");
            entity.HasIndex(e => e.UserId).HasDatabaseName("IX_UserOutput_UserId");
            entity.HasIndex(e => e.CalculatedAt).HasDatabaseName("IX_UserOutput_CalculatedAt");
        });
    }

    private void ConfigureRecommendationEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recommendation>(entity =>
        {
            entity.ToTable("UserRecommendation");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.UserId).HasColumnType("uniqueidentifier").IsRequired();
            entity.Property(e => e.BmiRecommendation).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(e => e.BmrRecommendation).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(e => e.TdeeRecommendation).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(e => e.BfpRecommendation).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(e => e.LbmRecommendation).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(e => e.WtHrRecommendation).HasColumnType("nvarchar(max)").IsRequired();
            entity.HasIndex(e => e.UserId).HasDatabaseName("IX_UserRecommendation_UserId");
        });
    }
}
