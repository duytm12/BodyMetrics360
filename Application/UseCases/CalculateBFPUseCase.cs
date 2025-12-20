using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.UseCases;

public class CalculateBFPUseCase(IGetBFP getBFP, IInputRepository inputRepository, IOutputRepository outputRepository)
{
    private readonly IGetBFP _getBFP = getBFP;
    private readonly IInputRepository _inputRepository = inputRepository;
    private readonly IOutputRepository _outputRepository = outputRepository;
    public async Task<CalculateBFPResponse> ExecuteAsync(CalculateBFPRequest request)
    {
        // 1. Get latest Input
        var existingInput = await _inputRepository.GetLatestAsync();

        // 2. Create new Input from request
        var newInput = new Input()
        {
            WaistInches = request.WaistInches,
            NeckInches = request.NeckInches,
            HeightInches = request.HeightInches,
            HipInches = request.HipInches,
            Gender = request.Gender
        };

        // 3. Merge
        var input = InputMergeService.MergeInput(existingInput, newInput);

        // 4. Save/update Input
        if (existingInput == null) input = await _inputRepository.AddAsync(input);

        else await _inputRepository.UpdateAsync(input);

        // 5. Perform calculations
        var bfp = await _getBFP.CalculateBFPAsync(input.WaistInches, input.NeckInches, input.HeightInches, input.HipInches, input.Gender);


        // 6. Get/create Output
        var existingOutput = await _outputRepository.GetByInputIdAsync(input.Id);
        Output output;

        // 7. Save/update Output
        if (existingOutput != null)
        {
            existingOutput.BFP = bfp;
            await _outputRepository.UpdateAsync(existingOutput);
            output = existingOutput;

        }
        else
        {
            output = new Output()
            {
                InputId = input.Id,
                CalculatedAt = DateTime.UtcNow,
                BFP = bfp
            };
            output = await _outputRepository.AddAsync(output);
        }

        // 8. Return response
        return new CalculateBFPResponse()
        {
            BFP = output.BFP
        };
    }
}
