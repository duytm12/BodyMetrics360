using Domain.Entities;

namespace TestProject;

public class TestInMemoryRepository
{
    [Fact]
    public async Task InputRepository_AddAsync_AssignsIdAndCreatedAt()
    {
        var repository = new InMemoryInputRepository();
        var input = new Input { WeightLbs = 150 };

        var stored = await repository.AddAsync(input);

        Assert.Equal(1, stored.Id);
        Assert.NotEqual(default, stored.CreatedAt);
        var latest = await repository.GetLatestAsync();
        Assert.Equal(stored.Id, latest!.Id);
    }

    [Fact]
    public async Task InputRepository_GetLatestAsync_ReturnsMostRecent()
    {
        var repository = new InMemoryInputRepository();
        var first = await repository.AddAsync(new Input { WeightLbs = 150 });
        await Task.Delay(10);
        var second = await repository.AddAsync(new Input { WeightLbs = 160 });

        var latest = await repository.GetLatestAsync();

        Assert.Equal(second.Id, latest!.Id);
        Assert.True(latest!.CreatedAt >= first.CreatedAt);
    }

    [Fact]
    public async Task InputRepository_UpdateAsync_UpdatesStoredInput()
    {
        var repository = new InMemoryInputRepository();
        var stored = await repository.AddAsync(new Input { WeightLbs = 150, WaistInches = 32 });

        stored.WaistInches = 34;
        await repository.UpdateAsync(stored);

        var latest = await repository.GetLatestAsync();
        Assert.Equal(34, latest!.WaistInches);
        Assert.Equal(stored.Id, latest.Id);
    }

    [Fact]
    public async Task OutputRepository_AddAsync_MapsByInputId()
    {
        var repository = new InMemoryOutputRepository();
        var output = new Output { InputId = 5, BMI = 25 };

        var stored = await repository.AddAsync(output);

        Assert.Equal(1, stored.Id);
        Assert.NotEqual(default, stored.CalculatedAt);
        var fetched = await repository.GetByInputIdAsync(5);
        Assert.Equal(stored.Id, fetched!.Id);
        Assert.Equal(25, fetched.BMI);
    }

    [Fact]
    public async Task OutputRepository_GetLatestAsync_ReturnsMostRecent()
    {
        var repository = new InMemoryOutputRepository();
        var first = await repository.AddAsync(new Output { InputId = 1, BMI = 20 });
        await Task.Delay(10);
        var second = await repository.AddAsync(new Output { InputId = 2, BMI = 22 });

        var latest = await repository.GetLatestAsync();

        Assert.Equal(second.Id, latest!.Id);
        Assert.True(latest.CalculatedAt >= first.CalculatedAt);
    }

    [Fact]
    public async Task OutputRepository_UpdateAsync_UpdatesStoredOutput()
    {
        var repository = new InMemoryOutputRepository();
        var stored = await repository.AddAsync(new Output { InputId = 3, BFP = 10 });

        stored.BFP = 12.5;
        await repository.UpdateAsync(stored);

        var fetched = await repository.GetByInputIdAsync(3);
        Assert.Equal(12.5, fetched!.BFP);
        Assert.Equal(stored.Id, fetched.Id);
    }
}
