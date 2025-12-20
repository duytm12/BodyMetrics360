using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces;

public interface IOutputRepository
{
    Task<Output?> GetLatestAsync();
    Task<Output?> GetByInputIdAsync(int inputId);
    Task<Output> AddAsync(Output output);
    Task UpdateAsync(Output output);
}
