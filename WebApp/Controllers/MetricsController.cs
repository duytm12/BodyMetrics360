using Application.DTOs;
using Application.Interfaces;
using Application.UseCases;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class MetricsController(
        IOutputRepository outputRepository,
        IInputRepository inputRepository,
        IRecommendationRepository recommendationRepository,
        IGetRecommendation getRecommendation,
        CalculateBMIUseCase calculateBMIUseCase,
        CalculateBFPUseCase calculateBFPUseCase,
        CalculateLBMUseCase calculateLBMUseCase,
        CalculateWtHRUseCase calculateWtHRUseCase,
        ISessionUserService sessionUserService) : Controller
    {
        private readonly IOutputRepository _outputRepository = outputRepository;
        private readonly IInputRepository _inputRepository = inputRepository;
        private readonly IRecommendationRepository _recommendationRepository = recommendationRepository;
        private readonly IGetRecommendation _getRecommendation = getRecommendation;
        private readonly CalculateBMIUseCase _calculateBMIUseCase = calculateBMIUseCase;
        private readonly CalculateBFPUseCase _calculateBFPUseCase = calculateBFPUseCase;
        private readonly CalculateLBMUseCase _calculateLBMUseCase = calculateLBMUseCase;
        private readonly CalculateWtHRUseCase _calculateWtHRUseCase = calculateWtHRUseCase;
        private readonly ISessionUserService _sessionUserService = sessionUserService;


        public async Task<IActionResult> Index()
        {
            var userId = _sessionUserService.GetCurrentUserId();
            var latestOutput = await _outputRepository.GetLatestByUserIdAsync(userId);
            var latestInput = await _inputRepository.GetLatestByUserIdAsync(userId);

            var summary = new MetricsSummary
            {
                BMI = latestOutput?.BMI,
                BMR = latestOutput?.BMR,
                TDEE = latestOutput?.TDEE,
                BFP = latestOutput?.BFP,
                LBM = latestOutput?.LBM,
                WtHR = latestOutput?.WtHR
            };

            if (latestOutput != null)
            {
                if (latestOutput.BMI > 0)
                {
                    var bmiReco = await _getRecommendation.GetBmiRecommendation(latestOutput.BMI);
                    summary.BmiRecommendation = new MetricRecommendation
                    {
                        Category = bmiReco.GetValueOrDefault("Category"),
                        Recommendation = bmiReco.GetValueOrDefault("Recommendation")
                    };
                }

                if (latestOutput.BMR > 0)
                {
                    var bmrText = await _getRecommendation.GetBmrRecommendation(latestOutput.BMR);
                    summary.BmrRecommendation = new MetricRecommendation
                    {
                        Category = null,
                        Recommendation = bmrText
                    };
                }

                if (latestOutput.TDEE > 0)
                {
                    var tdeeReco = await _getRecommendation.GetTdeeRecommendation(latestOutput.TDEE);
                    summary.TdeeRecommendation = new MetricRecommendation
                    {
                        Category = tdeeReco.GetValueOrDefault("Category"),
                        Recommendation = tdeeReco.GetValueOrDefault("Recommendation")
                    };
                }

                if (latestOutput.BFP > 0 && latestInput != null)
                {
                    var bfpReco = await _getRecommendation.GetBfpRecommendation(latestOutput.BFP, latestInput.Gender);
                    summary.BfpRecommendation = new MetricRecommendation
                    {
                        Category = bfpReco.GetValueOrDefault("Category"),
                        Recommendation = bfpReco.GetValueOrDefault("Recommendation")
                    };
                }

                if (latestOutput.LBM > 0 && latestInput != null)
                {
                    var lbmReco = await _getRecommendation.GetLbmRecommendation(latestOutput.LBM, latestInput.Gender);
                    summary.LbmRecommendation = new MetricRecommendation
                    {
                        Category = lbmReco.GetValueOrDefault("Category"),
                        Recommendation = lbmReco.GetValueOrDefault("Recommendation")
                    };
                }

                if (latestOutput.WtHR > 0)
                {
                    var wthrReco = await _getRecommendation.GetWthrRecommendation(latestOutput.WtHR);
                    summary.WtHrRecommendation = new MetricRecommendation
                    {
                        Category = wthrReco.GetValueOrDefault("Category"),
                        Recommendation = wthrReco.GetValueOrDefault("Recommendation")
                    };
                }
            }
            return View(summary);
        }

        [HttpGet]
        public async Task<IActionResult> CalculateBMI()
        {
            var userId = _sessionUserService.GetCurrentUserId();
            var latestInput = await _inputRepository.GetLatestByUserIdAsync(userId);
            var request = MetricsControllerHelper.MapToBMIRequest(latestInput);
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalculateBMI(CalculateBMIRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                var userId = _sessionUserService.GetCurrentUserId();
                var response = await _calculateBMIUseCase.ExecuteAsync(request, userId);
                TempData["BMIResult"] = response.BMI;
                TempData["BMRResult"] = response.BMR;
                TempData["TDEEResult"] = response.TDEE;

                // Recommendations for BMI and TDEE
                var bmiReco = await _getRecommendation.GetBmiRecommendation(response.BMI);
                TempData["BmiRecCategory"] = bmiReco.GetValueOrDefault("Category");
                TempData["BmiRecText"] = bmiReco.GetValueOrDefault("Recommendation");

                // Recommendation for BMR
                var bmrRecText = await _getRecommendation.GetBmrRecommendation(response.BMR);
                TempData["BmrRecText"] = bmrRecText;

                var tdeeReco = await _getRecommendation.GetTdeeRecommendation(response.TDEE);
                TempData["TdeeRecCategory"] = tdeeReco.GetValueOrDefault("Category");
                TempData["TdeeRecText"] = tdeeReco.GetValueOrDefault("Recommendation");

                 // Persist recommendation snapshot for this user (upsert - update existing or create new)
                var existing = await _recommendationRepository.GetByUserIdAsync(userId);
                var recommendation = new Recommendation
                {
                    UserId = userId,
                    BmiRecommendation = bmiReco.GetValueOrDefault("Recommendation") ?? string.Empty,
                    BmrRecommendation = bmrRecText,
                    TdeeRecommendation = tdeeReco.GetValueOrDefault("Recommendation") ?? string.Empty,
                    BfpRecommendation = existing?.BfpRecommendation ?? string.Empty,
                    LbmRecommendation = existing?.LbmRecommendation ?? string.Empty,
                    WtHrRecommendation = existing?.WtHrRecommendation ?? string.Empty
                };
                await _recommendationRepository.UpsertAsync(recommendation);

                TempData["SuccessMessage"] = "BMI, BMR, and TDEE calculated successfully!";
                return View(request);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error calculating metrics: {ex.Message}");
                return View(request);
            }
        }


        [HttpGet]
        public async Task<IActionResult> CalculateBFP()
        {
            var userId = _sessionUserService.GetCurrentUserId();
            var latestInput = await _inputRepository.GetLatestByUserIdAsync(userId);
            var request = MetricsControllerHelper.MapToBFPRequest(latestInput);
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalculateBFP(CalculateBFPRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                var userId = _sessionUserService.GetCurrentUserId();
                var response = await _calculateBFPUseCase.ExecuteAsync(request, userId);
                TempData["BFPResult"] = response.BFP;

                // Recommendation for BFP
                var bfpReco = await _getRecommendation.GetBfpRecommendation(response.BFP, request.Gender);
                TempData["BfpRecCategory"] = bfpReco.GetValueOrDefault("Category");
                TempData["BfpRecText"] = bfpReco.GetValueOrDefault("Recommendation");

                // Persist recommendation snapshot for this user (BFP-focused, upsert)
                var existing = await _recommendationRepository.GetByUserIdAsync(userId);
                var recommendation = new Recommendation
                {
                    UserId = userId,
                    BmiRecommendation = existing?.BmiRecommendation ?? string.Empty,
                    BmrRecommendation = existing?.BmrRecommendation ?? string.Empty,
                    TdeeRecommendation = existing?.TdeeRecommendation ?? string.Empty,
                    BfpRecommendation = bfpReco.GetValueOrDefault("Recommendation") ?? string.Empty,
                    LbmRecommendation = existing?.LbmRecommendation ?? string.Empty,
                    WtHrRecommendation = existing?.WtHrRecommendation ?? string.Empty
                };
                await _recommendationRepository.UpsertAsync(recommendation);

                TempData["SuccessMessage"] = "BFP calculated successfully!";
                return View(request);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error calculating BFP: {ex.Message}");
                return View(request);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CalculateLBM()
        {
            var userId = _sessionUserService.GetCurrentUserId();
            var latestInput = await _inputRepository.GetLatestByUserIdAsync(userId);
            var request = MetricsControllerHelper.MapToLBMRequest(latestInput);
            return View(request);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalculateLBM(CalculateLBMRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                var userId = _sessionUserService.GetCurrentUserId();
                var response = await _calculateLBMUseCase.ExecuteAsync(request, userId);
                TempData["LBMResult"] = response.LBM;

                // Recommendation for LBM
                var lbmReco = await _getRecommendation.GetLbmRecommendation(response.LBM, request.Gender);
                TempData["LbmRecCategory"] = lbmReco.GetValueOrDefault("Category");
                TempData["LbmRecText"] = lbmReco.GetValueOrDefault("Recommendation");

                // Persist recommendation snapshot for this user (LBM-focused, upsert)
                var existing = await _recommendationRepository.GetByUserIdAsync(userId);
                var recommendation = new Recommendation
                {
                    UserId = userId,
                    BmiRecommendation = existing?.BmiRecommendation ?? string.Empty,
                    BmrRecommendation = existing?.BmrRecommendation ?? string.Empty,
                    TdeeRecommendation = existing?.TdeeRecommendation ?? string.Empty,
                    BfpRecommendation = existing?.BfpRecommendation ?? string.Empty,
                    LbmRecommendation = lbmReco.GetValueOrDefault("Recommendation") ?? string.Empty,
                    WtHrRecommendation = existing?.WtHrRecommendation ?? string.Empty
                };
                await _recommendationRepository.UpsertAsync(recommendation);

                TempData["SuccessMessage"] = "LBM calculated successfully!";
                return View(request);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error calculating LBM: {ex.Message}");
                return View(request);
            }
        }


        [HttpGet]
        public async Task<IActionResult> CalculateWtHR()
        {
            var userId = _sessionUserService.GetCurrentUserId();
            var latestInput = await _inputRepository.GetLatestByUserIdAsync(userId);
            var request = MetricsControllerHelper.MapToWtHRRequest(latestInput);
            return View(request);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalculateWtHR(CalculateWtHRRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                var userId = _sessionUserService.GetCurrentUserId();
                var response = await _calculateWtHRUseCase.ExecuteAsync(request, userId);
                TempData["WtHRResult"] = response.WtHR;

                // Recommendation for WtHR
                var wthrReco = await _getRecommendation.GetWthrRecommendation(response.WtHR);
                TempData["WtHrRecCategory"] = wthrReco.GetValueOrDefault("Category");
                TempData["WtHrRecText"] = wthrReco.GetValueOrDefault("Recommendation");

                // Persist recommendation snapshot for this user (WtHR-focused, upsert)
                var existing = await _recommendationRepository.GetByUserIdAsync(userId);
                var recommendation = new Recommendation
                {
                    UserId = userId,
                    BmiRecommendation = existing?.BmiRecommendation ?? string.Empty,
                    BmrRecommendation = existing?.BmrRecommendation ?? string.Empty,
                    TdeeRecommendation = existing?.TdeeRecommendation ?? string.Empty,
                    BfpRecommendation = existing?.BfpRecommendation ?? string.Empty,
                    LbmRecommendation = existing?.LbmRecommendation ?? string.Empty,
                    WtHrRecommendation = wthrReco.GetValueOrDefault("Recommendation") ?? string.Empty
                };
                await _recommendationRepository.UpsertAsync(recommendation);

                TempData["SuccessMessage"] = "WtHR calculated successfully!";
                return View(request);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error calculating WtHR: {ex.Message}");
                return View(request);
            }

        }
    }
}
