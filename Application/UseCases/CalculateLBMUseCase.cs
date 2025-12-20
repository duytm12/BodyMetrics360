using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases;

public class CalculateLBMUseCase(IGetLBM getLBM, IInputRepository inputRepository, IOutputRepository outputRepository)
{
    private readonly IGetLBM _getLBM = getLBM;
    private readonly IInputRepository _inputRepository = inputRepository;
    private readonly IOutputRepository _outputRepository = outputRepository;

    public async Task<CalculateLBMResponse> ExecuteAsync(CalculateLBMRequest request)
    {
        var existingInput = await _inputRepository.GetLatestAsync();
        var newInput = new Input()
        {
            WeightLbs = request.WeightLbs,
            HeightInches = request.HeightInches,
            WaistInches = request.WaistInches,
            HipInches = request.HipInches,
            NeckInches = request.NeckInches,
            Gender = request.Gender
        };

        var input = InputMergeService.MergeInput(existingInput, newInput);

        if (existingInput == null) input = await _inputRepository.AddAsync(input);
        else await _inputRepository.UpdateAsync(input);

        var lbm = await _getLBM.CalculateLBMAsync(input.WeightLbs, input.HeightInches, input.NeckInches, input.WaistInches, input.HipInches, input.Gender);

        var existingOutput = await _outputRepository.GetByInputIdAsync(input.Id);
        Output output;

        if (existingOutput != null)
        {
            existingOutput.LBM = lbm;
            await _outputRepository.UpdateAsync(existingOutput);
            output = existingOutput;
        }
        else
        {
            output = new Output()
            {
                InputId = input.Id,
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
