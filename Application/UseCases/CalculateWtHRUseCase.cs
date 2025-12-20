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

    public async Task<CalculateWtHRResponse> ExecuteAsync(CalculateWtHRRequest request)
    {
        var existingInput = await _inputRepository.GetLatestAsync();
        var newInput = new Input()
        {
            WaistInches = request.WaistInches,
            HeightInches = request.HeightInches,
            Gender = request.Gender
        };

        var input = InputMergeService.MergeInput(existingInput, newInput);
        if (existingInput == null) input = await _inputRepository.AddAsync(input);
        else await _inputRepository.UpdateAsync(input);

        var wtHR = await _getWtHR.CalculateWtHRAsync(input.WaistInches, input.HeightInches);
        var existingOutput = await _outputRepository.GetByInputIdAsync(input.Id);
        Output output;

        if (existingOutput != null)
        {
            existingOutput.WtHR = wtHR;
            await _outputRepository.UpdateAsync(existingOutput);
            output = existingOutput;
        }
        else
        {
            output = new Output()
            {
                InputId = input.Id,
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
