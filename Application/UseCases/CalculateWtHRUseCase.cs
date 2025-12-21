using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases;

public class CalculateWtHRUseCase(IGetWtHR getWtHR, IInputRepository inputRepository, IOutputRepository outputRepository)
{
    private readonly IGetWtHR _getWtHR = getWtHR;
    private readonly IInputRepository _inputRepository = inputRepository;
    private readonly IOutputRepository _outputRepository = outputRepository;

    public async Task<CalculateWtHRResponse> ExecuteAsync(CalculateWtHRRequest request, Guid userId)
    {
        var existingInput = await _inputRepository.GetLatestByUserIdAsync(userId);
        var newInput = new Input()
        {
            UserId = userId,
            WaistInches = request.WaistInches,
            HeightInches = request.HeightInches,
            Gender = request.Gender
        };

        var input = InputMergeService.MergeInput(existingInput, newInput);
        input.UserId = userId; // Ensure userId is set after merge
        if (existingInput == null) input = await _inputRepository.AddAsync(input);
        else await _inputRepository.UpdateAsync(input);

        var wtHR = await _getWtHR.CalculateWtHRAsync(input.WaistInches, input.HeightInches);
        var existingOutput = await _outputRepository.GetByInputIdAsync(input.Id);
        Output output;

        if (existingOutput != null)
        {
            existingOutput.WtHR = wtHR;
            existingOutput.UserId = userId; // Ensure userId is set
            await _outputRepository.UpdateAsync(existingOutput);
            output = existingOutput;
        }
        else
        {
            output = new Output()
            {
                InputId = input.Id,
                UserId = userId,
                WtHR = wtHR
            };
            output = await _outputRepository.AddAsync(output);
        }
        return new CalculateWtHRResponse()
        {
            WtHR = output.WtHR
        };
    }
}
