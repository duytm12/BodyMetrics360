using Domain.Enums;
using Domain.Services;

namespace TestProject.TestDomain
{
    public class TestDomain
    {
        [Fact]
        public async Task CalculateBMIAsync_ReturnsExpectedValue()
        {
            var service = new GetBMI();

            var result = await service.CalculateBMIAsync(180, 70);

            Assert.Equal(25.82, result, 2);
        }

        [Fact]
        public async Task CalculateBMRAsync_ReturnsExpectedValue_ForMale()
        {
            var service = new GetBMI();

            var result = await service.CalculateBMRAsync(180, 70, 30, GenderEnum.Male);

            Assert.Equal(1783.08, result, 2);
        }

        [Fact]
        public async Task CalculateTDEEAsync_ReturnsExpectedValue_ForModeratelyActive()
        {
            var service = new GetBMI();
            var bmr = 1783.08;

            var result = await service.CalculateTDEEAsync(bmr, ActivityLevelEnum.ModeratelyActive);

            Assert.Equal(2763.80, result, 1);
        }

        [Fact]
        public async Task CalculateBMIAsync_ThrowsArgumentException_ForNonPositiveHeight()
        {
            var service = new GetBMI();

            await Assert.ThrowsAsync<ArgumentException>(() => service.CalculateBMIAsync(180, 0));
        }

        [Fact]
        public async Task CalculateBFPAsync_ReturnsExpectedValue_ForMale()
        {
            var bfpService = CreateInternalService("Domain.Services.GetBFP");
            var result = await bfpService.CalculateBFPAsync(34, 16, 70, 36, GenderEnum.Male);

            Assert.Equal(15.49, result, 2);
        }

        [Fact]
        public async Task CalculateBFPAsync_ReturnsExpectedValue_ForFemale()
        {
            var bfpService = CreateInternalService("Domain.Services.GetBFP");
            var result = await bfpService.CalculateBFPAsync(30, 13, 65, 38, GenderEnum.Female);

            Assert.Equal(28.56, result, 2);
        }

        [Fact]
        public async Task CalculateBFPAsync_ThrowsArgumentException_ForInvalidMeasurements()
        {
            var bfpService = CreateInternalService("Domain.Services.GetBFP");

            await Assert.ThrowsAsync<ArgumentException>(() => bfpService.CalculateBFPAsync(0, 13, 65, 38, GenderEnum.Female));
        }

        [Fact]
        public async Task CalculateLBMAsync_ReturnsExpectedValue_ForMale()
        {
            var lbmService = CreateInternalService("Domain.Services.GetLBM");
            var result = await lbmService.CalculateLBMAsync(160, 70, 16, 34, 36, GenderEnum.Male);

            Assert.Equal(135.21, result, 2);
        }

        [Fact]
        public async Task CalculateLBMAsync_ThrowsArgumentException_ForInvalidMeasurements()
        {
            var lbmService = CreateInternalService("Domain.Services.GetLBM");

            await Assert.ThrowsAsync<ArgumentException>(() => lbmService.CalculateLBMAsync(0, 70, 16, 34, 36, GenderEnum.Male));
        }

        [Fact]
        public async Task CalculateWtHRAsync_ReturnsExpectedValue()
        {
            var service = new GetWtHR();

            var result = await service.CalculateWtHRAsync(34, 70);

            Assert.Equal(0.49, result, 2);
        }

        [Fact]
        public async Task CalculateWtHRAsync_ThrowsArgumentException_ForInvalidInputs()
        {
            var service = new GetWtHR();

            await Assert.ThrowsAsync<ArgumentException>(() => service.CalculateWtHRAsync(-1, 70));
        }

        #region Recommendation Tests

        [Fact]
        public async Task GetBmiRecommendation_ReturnsUnderweight_ForLowBMI()
        {
            var service = new GetRecommendation();
            var result = await service.GetBmiRecommendation(17.0);

            Assert.Equal("Underweight", result["Category"]);
            Assert.Contains("gaining weight", result["Recommendation"]);
        }

        [Fact]
        public async Task GetBmiRecommendation_ReturnsNormalWeight_ForNormalBMI()
        {
            var service = new GetRecommendation();
            var result = await service.GetBmiRecommendation(22.0);

            Assert.Equal("Normal weight", result["Category"]);
            Assert.Contains("Maintain", result["Recommendation"]);
        }

        [Fact]
        public async Task GetBmiRecommendation_ReturnsOverweight_ForOverweightBMI()
        {
            var service = new GetRecommendation();
            var result = await service.GetBmiRecommendation(27.0);

            Assert.Equal("Overweight", result["Category"]);
            Assert.Contains("physical activity", result["Recommendation"]);
        }

        [Fact]
        public async Task GetBmiRecommendation_ReturnsObesityClassIII_ForHighBMI()
        {
            var service = new GetRecommendation();
            var result = await service.GetBmiRecommendation(42.0);

            Assert.Equal("Obesity class III", result["Category"]);
            Assert.Contains("Immediate medical intervention", result["Recommendation"]);
        }

        [Fact]
        public async Task GetBmrRecommendation_ReturnsFormattedMessage()
        {
            var service = new GetRecommendation();
            var result = await service.GetBmrRecommendation(1800.5);

            Assert.Contains("1800.50", result);
            Assert.Contains("calories/day", result);
            Assert.Contains("basic life functions", result);
        }

        [Fact]
        public async Task GetTdeeRecommendation_ReturnsCorrectFormat()
        {
            var service = new GetRecommendation();
            var result = await service.GetTdeeRecommendation(2500.0);

            Assert.Contains("2500.00", result["Category"]);
            Assert.Contains("2250.00", result["Recommendation"]); // lose weight
            Assert.Contains("2750.00", result["Recommendation"]); // gain weight
        }

        [Fact]
        public async Task GetBfpRecommendation_ReturnsAthletes_ForMaleAthleteBFP()
        {
            var service = new GetRecommendation();
            var result = await service.GetBfpRecommendation(10.0, GenderEnum.Male);

            Assert.Equal("Athletes", result["Category"]);
            Assert.Contains("training regimen", result["Recommendation"]);
        }

        [Fact]
        public async Task GetBfpRecommendation_ReturnsFitness_ForFemaleFitnessBFP()
        {
            var service = new GetRecommendation();
            var result = await service.GetBfpRecommendation(22.0, GenderEnum.Female);

            Assert.Equal("Fitness", result["Category"]);
            Assert.Contains("cardio and strength training", result["Recommendation"]);
        }

        [Fact]
        public async Task GetBfpRecommendation_ReturnsObese_ForHighBFP()
        {
            var service = new GetRecommendation();
            var resultMale = await service.GetBfpRecommendation(30.0, GenderEnum.Male);
            var resultFemale = await service.GetBfpRecommendation(35.0, GenderEnum.Female);

            Assert.Equal("Obese", resultMale["Category"]);
            Assert.Equal("Obese", resultFemale["Category"]);
            Assert.Contains("healthcare professional", resultMale["Recommendation"]);
        }

        [Fact]
        public async Task GetLbmRecommendation_ReturnsBelowAverage_ForLowLBM()
        {
            var service = new GetRecommendation();
            var result = await service.GetLbmRecommendation(65.0, GenderEnum.Male);

            Assert.Equal("Below Average", result["Category"]);
            Assert.Contains("strength training", result["Recommendation"]);
        }

        [Fact]
        public async Task GetLbmRecommendation_ReturnsAthletes_ForHighLBM()
        {
            var service = new GetRecommendation();
            var result = await service.GetLbmRecommendation(85.0, GenderEnum.Male);

            Assert.Equal("Athletes", result["Category"]);
            Assert.Contains("peak performance", result["Recommendation"]);
        }

        [Fact]
        public async Task GetLbmRecommendation_ReturnsCorrectCategory_ForFemale()
        {
            var service = new GetRecommendation();
            var result = await service.GetLbmRecommendation(70.0, GenderEnum.Female);

            Assert.Equal("Fitness Enthusiasts", result["Category"]);
        }

        [Fact]
        public async Task GetWthrRecommendation_ReturnsHealthy_ForHealthyWtHR()
        {
            var service = new GetRecommendation();
            var result = await service.GetWthrRecommendation(0.45);

            Assert.Equal("Healthy", result["Category"]);
            Assert.Contains("Maintain", result["Recommendation"]);
        }

        [Fact]
        public async Task GetWthrRecommendation_ReturnsObese_ForHighWtHR()
        {
            var service = new GetRecommendation();
            var result = await service.GetWthrRecommendation(0.65);

            Assert.Equal("Obese", result["Category"]);
            Assert.Contains("healthcare provider", result["Recommendation"]);
        }

        [Fact]
        public async Task GetWthrRecommendation_HandlesBoundaryValues()
        {
            var service = new GetRecommendation();
            
            var result1 = await service.GetWthrRecommendation(0.4);
            Assert.Equal("Underweight", result1["Category"]);

            var result2 = await service.GetWthrRecommendation(0.49);
            Assert.Equal("Healthy", result2["Category"]);

            var result3 = await service.GetWthrRecommendation(0.59);
            Assert.Equal("Overweight", result3["Category"]);

            var result4 = await service.GetWthrRecommendation(0.6);
            Assert.Equal("Obese", result4["Category"]);
        }

        #endregion

        private static dynamic CreateInternalService(string typeFullName)
        {
            var assembly = typeof(GetBMI).Assembly;
            var type = assembly.GetType(typeFullName) ?? throw new InvalidOperationException($"Type '{typeFullName}' not found.");
            return Activator.CreateInstance(type, nonPublic: true)!;
        }
    }
}
