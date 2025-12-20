using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.InMemory;

public class InMemoryInputRepository : IInputRepository
{
    private readonly ConcurrentDictionary<int, Input> _inputs = new();
    private int _nextId = 1;
    private readonly object _lockObject = new();
    public Task<Input> AddAsync(Input input)
    {
        lock (_lockObject)
        {
            input.Id = _nextId++;
            input.CreatedAt = DateTime.UtcNow;
            _inputs.TryAdd(input.Id, input);
        }
        return Task.FromResult(input);
    }

    public Task<Input?> GetLatestAsync()
    {
        var input = _inputs.Values.OrderByDescending(i => i.CreatedAt).FirstOrDefault();
        return Task.FromResult<Input?>(input);
    }

    public Task UpdateAsync(Input input)
    {
        _inputs.AddOrUpdate(input.Id, input, (key, oldValue) => input);
        return Task.CompletedTask;
    }
}
