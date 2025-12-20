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

        private static dynamic CreateInternalService(string typeFullName)
        {
            var assembly = typeof(GetBMI).Assembly;
            var type = assembly.GetType(typeFullName) ?? throw new InvalidOperationException($"Type '{typeFullName}' not found.");
            return Activator.CreateInstance(type, nonPublic: true)!;
        }
    }
}
