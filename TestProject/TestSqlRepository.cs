using Domain.Entities;
using Infrastructure.SQLServer;
using Microsoft.EntityFrameworkCore;

namespace TestProject;

public class TestSqlRepository
{
    private static DbContextOptions<AppDbContext> CreateOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task InputRepository_AddsAndSetsCreatedAt()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerInputRepository(context);

        var input = new Input { UserId = Guid.NewGuid(), WeightLbs = 70, HeightInches = 175, Age = 30 };

        var saved = await repo.AddAsync(input);

        Assert.NotEqual(default, saved.CreatedAt);
        Assert.Equal(saved.Id, (await context.Inputs.SingleAsync()).Id);
    }

    [Fact]
    public async Task InputRepository_GetLatestByUserId_ReturnsMostRecentForUser()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerInputRepository(context);
        var userId = Guid.NewGuid();

        await repo.AddAsync(new Input { UserId = userId, WeightLbs = 70, HeightInches = 175, Age = 30, CreatedAt = DateTime.UtcNow.AddMinutes(-5) });
        await repo.AddAsync(new Input { UserId = userId, WeightLbs = 71, HeightInches = 176, Age = 31, CreatedAt = DateTime.UtcNow });

        var latest = await repo.GetLatestByUserIdAsync(userId);

        Assert.NotNull(latest);
        Assert.Equal(71, latest!.WeightLbs);
        Assert.Equal(userId, latest.UserId);
    }

    [Fact]
    public async Task InputRepository_Update_PersistsChanges()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerInputRepository(context);

        var saved = await repo.AddAsync(new Input { UserId = Guid.NewGuid(), WeightLbs = 70, HeightInches = 175, Age = 30 });
        saved.WeightLbs = 75;

        await repo.UpdateAsync(saved);

        var reloaded = await context.Inputs.SingleAsync();
        Assert.Equal(75, reloaded.WeightLbs);
    }

    [Fact]
    public async Task OutputRepository_AddsAndSetsCalculatedAt()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerOutputRepository(context);

        var output = new Output { UserId = Guid.NewGuid(), InputId = 1, BMI = 22.5 };

        var saved = await repo.AddAsync(output);

        Assert.NotEqual(default, saved.CalculatedAt);
        Assert.Equal(saved.Id, (await context.Outputs.SingleAsync()).Id);
    }

    [Fact]
    public async Task OutputRepository_GetByInputId_ReturnsMatch()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerOutputRepository(context);

        var userId = Guid.NewGuid();
        await repo.AddAsync(new Output { UserId = userId, InputId = 1, BMI = 22.5 });
        await repo.AddAsync(new Output { UserId = userId, InputId = 2, BMI = 23.5 });

        var result = await repo.GetByInputIdAsync(2);

        Assert.NotNull(result);
        Assert.Equal(23.5, result!.BMI);
    }

    [Fact]
    public async Task OutputRepository_GetLatestByUserId_ReturnsMostRecentForUser()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerOutputRepository(context);
        var userId = Guid.NewGuid();

        await repo.AddAsync(new Output { UserId = userId, InputId = 1, BMI = 22.5, CalculatedAt = DateTime.UtcNow.AddMinutes(-3) });
        await repo.AddAsync(new Output { UserId = userId, InputId = 2, BMI = 23.0, CalculatedAt = DateTime.UtcNow });

        var latest = await repo.GetLatestByUserIdAsync(userId);

        Assert.NotNull(latest);
        Assert.Equal(2, latest!.InputId);
        Assert.Equal(userId, latest.UserId);
    }

    [Fact]
    public async Task OutputRepository_Update_PersistsChanges()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerOutputRepository(context);

        var saved = await repo.AddAsync(new Output { UserId = Guid.NewGuid(), InputId = 1, BMI = 22.5 });
        saved.BMI = 24.0;

        await repo.UpdateAsync(saved);

        var reloaded = await context.Outputs.SingleAsync();
        Assert.Equal(24.0, reloaded.BMI);
    }

    [Fact]
    public async Task RecommendationRepository_AddAsync_CreatesNewRecommendation()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerRecommendationRepository(context);

        var recommendation = new Recommendation
        {
            UserId = Guid.NewGuid(),
            BmiRecommendation = "Test BMI recommendation",
            BmrRecommendation = "Test BMR recommendation",
            TdeeRecommendation = "Test TDEE recommendation",
            BfpRecommendation = string.Empty,
            LbmRecommendation = string.Empty,
            WtHrRecommendation = string.Empty
        };

        var saved = await repo.AddAsync(recommendation);

        Assert.NotEqual(0, saved.Id);
        Assert.Equal(saved.Id, (await context.Recommendations.SingleAsync()).Id);
        Assert.Equal("Test BMI recommendation", saved.BmiRecommendation);
    }

    [Fact]
    public async Task RecommendationRepository_GetByUserIdAsync_ReturnsRecommendation_WhenExists()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerRecommendationRepository(context);
        var userId = Guid.NewGuid();

        await repo.AddAsync(new Recommendation
        {
            UserId = userId,
            BmiRecommendation = "BMI rec",
            BmrRecommendation = "BMR rec",
            TdeeRecommendation = "TDEE rec",
            BfpRecommendation = string.Empty,
            LbmRecommendation = string.Empty,
            WtHrRecommendation = string.Empty
        });

        var result = await repo.GetByUserIdAsync(userId);

        Assert.NotNull(result);
        Assert.Equal(userId, result!.UserId);
        Assert.Equal("BMI rec", result.BmiRecommendation);
    }

    [Fact]
    public async Task RecommendationRepository_GetByUserIdAsync_ReturnsNull_WhenNotExists()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerRecommendationRepository(context);

        var result = await repo.GetByUserIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task RecommendationRepository_UpsertAsync_CreatesNew_WhenNotExists()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerRecommendationRepository(context);
        var userId = Guid.NewGuid();

        var recommendation = new Recommendation
        {
            UserId = userId,
            BmiRecommendation = "New BMI",
            BmrRecommendation = "New BMR",
            TdeeRecommendation = "New TDEE",
            BfpRecommendation = string.Empty,
            LbmRecommendation = string.Empty,
            WtHrRecommendation = string.Empty
        };

        var result = await repo.UpsertAsync(recommendation);

        Assert.NotEqual(0, result.Id);
        Assert.Equal(userId, result.UserId);
        Assert.Equal("New BMI", result.BmiRecommendation);
        var count = await context.Recommendations.CountAsync();
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task RecommendationRepository_UpsertAsync_UpdatesExisting_WhenExists()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerRecommendationRepository(context);
        var userId = Guid.NewGuid();

        var initial = await repo.AddAsync(new Recommendation
        {
            UserId = userId,
            BmiRecommendation = "Initial BMI",
            BmrRecommendation = "Initial BMR",
            TdeeRecommendation = "Initial TDEE",
            BfpRecommendation = string.Empty,
            LbmRecommendation = string.Empty,
            WtHrRecommendation = string.Empty
        });

        var initialId = initial.Id;

        var updated = new Recommendation
        {
            UserId = userId,
            BmiRecommendation = "Updated BMI",
            BmrRecommendation = "Updated BMR",
            TdeeRecommendation = "Updated TDEE",
            BfpRecommendation = "Updated BFP",
            LbmRecommendation = "Updated LBM",
            WtHrRecommendation = "Updated WtHR"
        };

        var result = await repo.UpsertAsync(updated);

        Assert.Equal(initialId, result.Id); // Same ID
        Assert.Equal("Updated BMI", result.BmiRecommendation);
        Assert.Equal("Updated BMR", result.BmrRecommendation);
        Assert.Equal("Updated TDEE", result.TdeeRecommendation);
        Assert.Equal("Updated BFP", result.BfpRecommendation);
        Assert.Equal("Updated LBM", result.LbmRecommendation);
        Assert.Equal("Updated WtHR", result.WtHrRecommendation);

        var count = await context.Recommendations.CountAsync();
        Assert.Equal(1, count); // Still only one row

        var retrieved = await repo.GetByUserIdAsync(userId);
        Assert.NotNull(retrieved);
        Assert.Equal("Updated BMI", retrieved!.BmiRecommendation);
    }

    [Fact]
    public async Task RecommendationRepository_UpsertAsync_MaintainsOneRowPerUser()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerRecommendationRepository(context);
        var userId = Guid.NewGuid();

        // First upsert
        await repo.UpsertAsync(new Recommendation
        {
            UserId = userId,
            BmiRecommendation = "First BMI",
            BmrRecommendation = string.Empty,
            TdeeRecommendation = string.Empty,
            BfpRecommendation = string.Empty,
            LbmRecommendation = string.Empty,
            WtHrRecommendation = string.Empty
        });

        // Second upsert
        await repo.UpsertAsync(new Recommendation
        {
            UserId = userId,
            BmiRecommendation = "Second BMI",
            BmrRecommendation = string.Empty,
            TdeeRecommendation = string.Empty,
            BfpRecommendation = "Second BFP",
            LbmRecommendation = string.Empty,
            WtHrRecommendation = string.Empty
        });

        // Third upsert
        await repo.UpsertAsync(new Recommendation
        {
            UserId = userId,
            BmiRecommendation = "Third BMI",
            BmrRecommendation = string.Empty,
            TdeeRecommendation = string.Empty,
            BfpRecommendation = "Third BFP",
            LbmRecommendation = "Third LBM",
            WtHrRecommendation = string.Empty
        });

        var count = await context.Recommendations.CountAsync();
        Assert.Equal(1, count); // Only one row per user

        var final = await repo.GetByUserIdAsync(userId);
        Assert.NotNull(final);
        Assert.Equal("Third BMI", final!.BmiRecommendation);
        Assert.Equal("Third BFP", final.BfpRecommendation);
        Assert.Equal("Third LBM", final.LbmRecommendation);
    }
}
