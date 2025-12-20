using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases;

public class CalculateBMIUseCase(IGetBMI getBMI, IInputRepository inputRepository, IOutputRepository outputRepository)
{
    private readonly IGetBMI _getBMI = getBMI;
    private readonly IInputRepository _inputRepository = inputRepository;
    private readonly IOutputRepository _outputRepository = outputRepository;

    public async Task<CalculateBMIResponse> ExecuteAsync(CalculateBMIRequest request)
    {
        // 1. Get latest Input
        var existingInput = await _inputRepository.GetLatestAsync();

        // 2. Create new Input from request
        var newInput = new Input
        {
            WeightLbs = request.WeightLbs,
            HeightInches = request.HeightInches,
            Age = request.Age,
            Gender = request.Gender,
            ActivityLevel = request.ActivityLevel
        };

        // 3. Merge Inputs
        var input = InputMergeService.MergeInput(existingInput, newInput);

        // 4. Save/update Input
        if (existingInput == null)  input = await _inputRepository.AddAsync(input);

        else await _inputRepository.UpdateAsync(input);

        // 5. Perform Calculations
        var bmi = await _getBMI.CalculateBMIAsync(input.WeightLbs, input.HeightInches);
        var bmr = await _getBMI.CalculateBMRAsync(input.WeightLbs, input.HeightInches, input.Age, input.Gender);
        var tdee = await _getBMI.CalculateTDEEAsync(bmr, input.ActivityLevel);

        // 6. Get or Create Output entity
        var existingOutput = await _outputRepository.GetByInputIdAsync(input.Id);
        Output output;

        // 7. Save/update Output
        if (existingOutput != null)
        {
            existingOutput.BMI = bmi;
            existingOutput.BMR = bmr;
            existingOutput.TDEE = tdee;
            await _outputRepository.UpdateAsync(existingOutput);
            output = existingOutput;
        }
        else
        {
            output = new Output
            {
                InputId = input.Id,
                BMI = bmi,
                BMR = bmr,
                TDEE = tdee
            };
            output = await _outputRepository.AddAsync(output);

        }

        return new CalculateBMIResponse
        {
            BMI = output.BMI,
            BMR = output.BMR,
            TDEE = output.TDEE
        };
    }
}
