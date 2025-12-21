using Application.Interfaces;
using Application.UseCases;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class MetricsController(
        IOutputRepository outputRepository, IInputRepository inputRepository,
        CalculateBMIUseCase calculateBMIUseCase, CalculateBFPUseCase calculateBFPUseCase,
        CalculateLBMUseCase calculateLBMUseCase, CalculateWtHRUseCase calculateWtHRUseCase,
        ISessionUserService sessionUserService) : Controller
    {
        private readonly IOutputRepository _outputRepository = outputRepository;
        private readonly IInputRepository _inputRepository = inputRepository;
        private readonly CalculateBMIUseCase _calculateBMIUseCase = calculateBMIUseCase;
        private readonly CalculateBFPUseCase _calculateBFPUseCase = calculateBFPUseCase;
        private readonly CalculateLBMUseCase _calculateLBMUseCase = calculateLBMUseCase;
        private readonly CalculateWtHRUseCase _calculateWtHRUseCase = calculateWtHRUseCase;
        private readonly ISessionUserService _sessionUserService = sessionUserService;


        public async Task<IActionResult> Index()
        {
            var userId = _sessionUserService.GetCurrentUserId();
            var latestOutput = await _outputRepository.GetLatestByUserIdAsync(userId);

            var summary = new MetricsSummary
            {
                BMI = latestOutput?.BMI,
                BMR = latestOutput?.BMR,
                TDEE = latestOutput?.TDEE,
                BFP = latestOutput?.BFP,
                LBM = latestOutput?.LBM,
                WtHR = latestOutput?.WtHR
            };
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
