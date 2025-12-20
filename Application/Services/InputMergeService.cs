using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services;

public class InputMergeService
{
    public static Input MergeInput(Input? existingInput, Input newInput)
    {
        if (existingInput == null) return newInput;

        // Merge logic (example: prefer newInput values)
        if (newInput.WeightLbs > 0) existingInput.WeightLbs = newInput.WeightLbs;
        if (newInput.HeightInches > 0) existingInput.HeightInches = newInput.HeightInches;
        if (newInput.WaistInches > 0) existingInput.WaistInches = newInput.WaistInches;
        if (newInput.NeckInches > 0) existingInput.NeckInches = newInput.NeckInches;
        if (newInput.HipInches > 0) existingInput.HipInches = newInput.HipInches;
        if (newInput.Age > 0) existingInput.Age = newInput.Age;
        existingInput.Gender = newInput.Gender;
        existingInput.ActivityLevel = newInput.ActivityLevel;

        return existingInput;
    }
}
