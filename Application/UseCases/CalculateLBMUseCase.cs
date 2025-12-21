using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases;

public class CalculateLBMUseCase(IGetLBM getLBM, IInputRepository inputRepository, IOutputRepository outputRepository)
{
    private readonly IGetLBM _getLBM = getLBM;
    private readonly IInputRepository _inputRepository = inputRepository;
    private readonly IOutputRepository _outputRepository = outputRepository;

    public async Task<CalculateLBMResponse> ExecuteAsync(CalculateLBMRequest request, Guid userId)
    {
        var existingInput = await _inputRepository.GetLatestByUserIdAsync(userId);
        var newInput = new Input()
        {
            UserId = userId,
            WeightLbs = request.WeightLbs,
            HeightInches = request.HeightInches,
            WaistInches = request.WaistInches,
            HipInches = request.HipInches,
            NeckInches = request.NeckInches,
            Gender = request.Gender
        };

        var input = InputMergeService.MergeInput(existingInput, newInput);
        input.UserId = userId; // Ensure userId is set after merge

        if (existingInput == null) input = await _inputRepository.AddAsync(input);
        else await _inputRepository.UpdateAsync(input);

        var lbm = await _getLBM.CalculateLBMAsync(input.WeightLbs, input.HeightInches, input.NeckInches, input.WaistInches, input.HipInches, input.Gender);

        var existingOutput = await _outputRepository.GetByInputIdAsync(input.Id);
        Output output;

        if (existingOutput != null)
        {
            existingOutput.LBM = lbm;
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
                LBM = lbm
            };
            output = await _outputRepository.AddAsync(output);

        }

        return new CalculateLBMResponse()
        {
            LBM = output.LBM
        };


    }
}
