using Application.DTOs;
using Application.Services;
using Application.UseCases;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Services;

namespace TestProject
{
    public class TestApplication
    {
        [Fact]
        public void MergeInput_UsesNonZeroValuesAndOverridesGenderAndActivity()
        {
            var existing = new Input
            {
                WeightLbs = 150,
                HeightInches = 68,
                WaistInches = 32,
                NeckInches = 15,
                HipInches = 36,
                Age = 28,
                Gender = GenderEnum.Female,
                ActivityLevel = ActivityLevelEnum.LightlyActive
            };

            var incoming = new Input
            {
                WeightLbs = 0,
                HeightInches = 70,
                WaistInches = 34,
                NeckInches = 0,
                HipInches = 0,
                Age = 0,
                Gender = GenderEnum.Male,
                ActivityLevel = ActivityLevelEnum.ModeratelyActive
            };

            var merged = InputMergeService.MergeInput(existing, incoming);

            Assert.Equal(150, merged.WeightLbs);
            Assert.Equal(70, merged.HeightInches);
            Assert.Equal(34, merged.WaistInches);
            Assert.Equal(15, merged.NeckInches);
            Assert.Equal(36, merged.HipInches);
            Assert.Equal(28, merged.Age);
            Assert.Equal(GenderEnum.Male, merged.Gender);
            Assert.Equal(ActivityLevelEnum.ModeratelyActive, merged.ActivityLevel);
        }

        [Fact]
        public async Task CalculateBMIUseCase_ComputesAndStoresOutputs()
        {
            var inputRepository = new InMemoryInputRepository();
            var outputRepository = new InMemoryOutputRepository();
            var useCase = new CalculateBMIUseCase(new GetBMI(), inputRepository, outputRepository);

            var testUserId = Guid.NewGuid();
            var response = await useCase.ExecuteAsync(new CalculateBMIRequest
            {
                WeightLbs = 180,
                HeightInches = 70,
                Age = 30,
                Gender = GenderEnum.Male,
                ActivityLevel = ActivityLevelEnum.ModeratelyActive
            }, testUserId);

            Assert.Equal(25.82, response.BMI, 2);
            Assert.Equal(1783.08, response.BMR, 2);
            Assert.Equal(2763.80, response.TDEE, 1);

            var storedOutput = outputRepository.LastOutput!;
            Assert.Equal(response.BMI, storedOutput.BMI, 3);
            Assert.Equal(response.BMR, storedOutput.BMR, 3);
            Assert.Equal(response.TDEE, storedOutput.TDEE, 3);
            Assert.Equal(inputRepository.LastInput!.Id, storedOutput.InputId);
        }

        [Fact]
        public async Task CalculateBFPUseCase_ComputesBodyFatPercentage()
        {
            var inputRepository = new InMemoryInputRepository();
            var outputRepository = new InMemoryOutputRepository();
            var useCase = new CalculateBFPUseCase(new GetBFP(), inputRepository, outputRepository);

            var testUserId = Guid.NewGuid();
            var response = await useCase.ExecuteAsync(new CalculateBFPRequest
            {
                WaistInches = 34,
                NeckInches = 16,
                HeightInches = 70,
                HipInches = 36,
                Gender = GenderEnum.Male
            }, testUserId);

            Assert.Equal(15.49, response.BFP, 2);
            Assert.Equal(response.BFP, outputRepository.LastOutput!.BFP, 3);
        }

        [Fact]
        public async Task CalculateLBMUseCase_ComputesLeanBodyMass()
        {
            var inputRepository = new InMemoryInputRepository();
            var outputRepository = new InMemoryOutputRepository();
            var useCase = new CalculateLBMUseCase(new GetLBM(), inputRepository, outputRepository);

            var testUserId = Guid.NewGuid();
            var response = await useCase.ExecuteAsync(new CalculateLBMRequest
            {
                WeightLbs = 160,
                HeightInches = 70,
                WaistInches = 34,
                HipInches = 36,
                NeckInches = 16,
                Gender = GenderEnum.Male
            }, testUserId);

            Assert.Equal(135.21, response.LBM, 2);
            Assert.Equal(response.LBM, outputRepository.LastOutput!.LBM, 3);
        }

        [Fact]
        public async Task CalculateWtHRUseCase_ComputesWaistToHeightRatio()
        {
            var inputRepository = new InMemoryInputRepository();
            var outputRepository = new InMemoryOutputRepository();
            var useCase = new CalculateWtHRUseCase(new GetWtHR(), inputRepository, outputRepository);

            var testUserId = Guid.NewGuid();
            var response = await useCase.ExecuteAsync(new CalculateWtHRRequest
            {
                WaistInches = 34,
                HeightInches = 70
            }, testUserId);

            Assert.Equal(0.49, response.WtHR, 2);
            Assert.Equal(response.WtHR, outputRepository.LastOutput!.WtHR, 3);
        }
    }

    internal class InMemoryInputRepository : IInputRepository
    {
        private int _nextId = 1;
        public Input? LastInput { get; private set; }

        public Task<Input?> GetLatestByUserIdAsync(Guid userId) => Task.FromResult(LastInput?.UserId == userId ? LastInput : null);

        public Task<Input> AddAsync(Input input)
        {
            input.Id = _nextId++;
            input.CreatedAt = DateTime.UtcNow;
            LastInput = input;
            return Task.FromResult(input);
        }

        public Task UpdateAsync(Input input)
        {
            LastInput = input;
            return Task.CompletedTask;
        }
    }

    internal class InMemoryOutputRepository : IOutputRepository
    {
        private int _nextId = 1;
        private readonly Dictionary<int, Output> _outputs = new();
        public Output? LastOutput { get; private set; }

        public Task<Output?> GetLatestByUserIdAsync(Guid userId) => Task.FromResult(LastOutput?.UserId == userId ? LastOutput : null);

        public Task<Output?> GetByInputIdAsync(int inputId)
        {
            _outputs.TryGetValue(inputId, out var output);
            return Task.FromResult(output);
        }

        public Task<Output> AddAsync(Output output)
        {
            output.Id = _nextId++;
            output.CalculatedAt = output.CalculatedAt == default ? DateTime.UtcNow : output.CalculatedAt;
            _outputs[output.InputId] = output;
            LastOutput = output;
            return Task.FromResult(output);
        }

        public Task UpdateAsync(Output output)
        {
            _outputs[output.InputId] = output;
            LastOutput = output;
            return Task.CompletedTask;
        }
    }
}
