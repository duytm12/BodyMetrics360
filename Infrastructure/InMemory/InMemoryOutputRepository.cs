using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Concurrent;

namespace Infrastructure.InMemory;

public class InMemoryOutputRepository : IOutputRepository
{
    private readonly ConcurrentDictionary<int, Output> _outputs = new();

    // Mapping from InputId to OutputId
    private readonly ConcurrentDictionary<int, int> _inputIdToOutputId = new();

    private int _nextId = 1;
    private readonly object _lockObject = new();

    public Task<Output> AddAsync(Output output)
    {
        lock (_lockObject)
        {
            output.Id = _nextId++;
            output.CalculatedAt = DateTime.UtcNow;
            _outputs.TryAdd(output.Id, output);
            _inputIdToOutputId.AddOrUpdate(output.InputId, output.Id, (key, oldValue) => output.Id);
        }
        return Task.FromResult(output);
    }

    public Task<Output?> GetByInputIdAsync(int inputId)
    {
        if (_inputIdToOutputId.TryGetValue(inputId, out var outputId))
        {
            _outputs.TryGetValue(outputId, out var output);
            return Task.FromResult(output);
        }
        return Task.FromResult<Output?>(null);
    }

    public Task<Output?> GetLatestAsync()
    {
        var output = _outputs.Values.OrderByDescending(o => o.CalculatedAt).FirstOrDefault();
        return Task.FromResult(output);
    }

    public Task UpdateAsync(Output output)
    {
        _outputs.AddOrUpdate(output.Id, output, (key, oldValue) => output);
        _inputIdToOutputId.AddOrUpdate(output.InputId, output.Id, (key, oldValue) => output.Id);
        return Task.CompletedTask;
    }
}
