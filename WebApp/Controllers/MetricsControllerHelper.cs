using Domain.Entities;

namespace WebApp.Controllers;

public static class MetricsControllerHelper
{
    public static CalculateBMIRequest MapToBMIRequest(Input? input)
    {
        var request = new CalculateBMIRequest();
        if (input != null)
        {
            request.WeightLbs = input.WeightLbs > 0 ? input.WeightLbs : 0;
            request.HeightInches = input.HeightInches > 0 ? input.HeightInches : 0;
            request.Age = input.Age > 0 ? input.Age : 0;
            request.Gender = input.Gender;
            request.ActivityLevel = input.ActivityLevel;
        }

        return request;
    }

    public static CalculateBFPRequest MapToBFPRequest(Input? input)
    {
        var request = new CalculateBFPRequest();
        if (input != null)
        {
            request.WaistInches = input.WaistInches > 0 ? input.WaistInches : 0;
            request.NeckInches = input.NeckInches > 0 ? input.NeckInches : 0;
            request.HipInches = input.HipInches > 0 ? input.HipInches : 0;
            request.HeightInches = input.HeightInches > 0 ? input.HeightInches : 0;
            request.Gender = input.Gender;
        }
        return request;
    }

    public static CalculateLBMRequest MapToLBMRequest(Input? input)
    {
        var request = new CalculateLBMRequest();
        if (input != null)
        {
            request.WeightLbs = input.WeightLbs > 0 ? input.WeightLbs : 0;
            request.HeightInches = input.HeightInches > 0 ? input.HeightInches : 0;
            request.WaistInches = input.WaistInches > 0 ? input.WaistInches : 0;
            request.NeckInches = input.NeckInches > 0 ? input.NeckInches : 0;
            request.HipInches = input.HipInches > 0 ? input.HipInches : 0;
            request.Gender = input.Gender;
        }

        return request;
    }

    public static CalculateWtHRRequest MapToWtHRRequest(Input? input)
    {
        var request = new CalculateWtHRRequest();
        if (input != null)
        {
            request.WaistInches = input.WaistInches > 0 ? input.WaistInches : 0;
            request.HeightInches = input.HeightInches > 0 ? input.HeightInches : 0;
            request.Gender = input.Gender;
        }
        return request;
    }
}
