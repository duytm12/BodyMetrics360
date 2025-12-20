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

        var input = new Input { WeightLbs = 70, HeightInches = 175, Age = 30 };

        var saved = await repo.AddAsync(input);

        Assert.NotEqual(default, saved.CreatedAt);
        Assert.Equal(saved.Id, (await context.Inputs.SingleAsync()).Id);
    }

    [Fact]
    public async Task InputRepository_GetLatest_ReturnsMostRecent()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerInputRepository(context);

        await repo.AddAsync(new Input { WeightLbs = 70, HeightInches = 175, Age = 30, CreatedAt = DateTime.UtcNow.AddMinutes(-5) });
        await repo.AddAsync(new Input { WeightLbs = 71, HeightInches = 176, Age = 31, CreatedAt = DateTime.UtcNow });

        var latest = await repo.GetLatestAsync();

        Assert.NotNull(latest);
        Assert.Equal(71, latest!.WeightLbs);
    }

    [Fact]
    public async Task InputRepository_Update_PersistsChanges()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerInputRepository(context);

        var saved = await repo.AddAsync(new Input { WeightLbs = 70, HeightInches = 175, Age = 30 });
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

        var output = new Output { InputId = 1, BMI = 22.5 };

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

        await repo.AddAsync(new Output { InputId = 1, BMI = 22.5 });
        await repo.AddAsync(new Output { InputId = 2, BMI = 23.5 });

        var result = await repo.GetByInputIdAsync(2);

        Assert.NotNull(result);
        Assert.Equal(23.5, result!.BMI);
    }

    [Fact]
    public async Task OutputRepository_GetLatest_ReturnsMostRecent()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerOutputRepository(context);

        await repo.AddAsync(new Output { InputId = 1, BMI = 22.5, CalculatedAt = DateTime.UtcNow.AddMinutes(-3) });
        await repo.AddAsync(new Output { InputId = 2, BMI = 23.0, CalculatedAt = DateTime.UtcNow });

        var latest = await repo.GetLatestAsync();

        Assert.NotNull(latest);
        Assert.Equal(2, latest!.InputId);
    }

    [Fact]
    public async Task OutputRepository_Update_PersistsChanges()
    {
        var options = CreateOptions();
        await using var context = new AppDbContext(options);
        var repo = new SqlServerOutputRepository(context);

        var saved = await repo.AddAsync(new Output { InputId = 1, BMI = 22.5 });
        saved.BMI = 24.0;

        await repo.UpdateAsync(saved);

        var reloaded = await context.Outputs.SingleAsync();
        Assert.Equal(24.0, reloaded.BMI);
    }
}
