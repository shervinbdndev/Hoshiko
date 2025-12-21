using System.Security.Claims;
using Hoshiko.Core.Interfaces;
using Hoshiko.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Hoshiko.Web.Controllers
{
    
    [Authorize]
    [Route("Stages")]
    public class StageController : Controller
    {
        private readonly IStageRepository _stageRepo;
        private readonly IStageProgressService _progressService;
        private readonly ICertificateService _certificateService;

        public StageController(
            IStageRepository stageRepo,
            IStageProgressService progressService,
            ICertificateService certificateService
        )
        {
            _stageRepo = stageRepo;
            _progressService = progressService;
            _certificateService = certificateService;
        }


        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        private async Task<Stage?> GetStageSlug(string stageSlug)
        {
            return await _stageRepo.GetBySlugAsync(stageSlug);
        }
        


        private async Task<IActionResult?> CheckAccess(int stageId)
        {
            var userId = GetUserId();

            if (!await _progressService.CanAccessStageAsync(userId, stageId)) return Forbid();

            return null;
        }




        [HttpGet(nameof(Start))]
        public async Task<IActionResult> Start()
        {
            var userId = GetUserId();
            var nextStage = await _progressService.GetNextStageForUserAsync(userId);

            if (nextStage == null)
            {
                var cert = await _certificateService.TryIssueCertificateAsync(userId);
                return View(nameof(Completed), cert);
            }

            return RedirectToAction(nameof(Learn), new { stageSlug = nextStage.Slug });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Retry()
        {
            var userId = GetUserId();

            await _progressService.ResetQuizProgressWithLimitAsync(userId);

            return RedirectToAction(nameof(Start));
        }




        [HttpGet("{stageSlug}/Learn")]
        public async Task<IActionResult> Learn(string stageSlug)
        {
            var stage = await GetStageSlug(stageSlug);
            if (stage == null) return NotFound();

            var deny = await CheckAccess(stage.Id);
            if (deny != null) return deny;

            var fullStage = await _stageRepo.GetWithQuizzesAsync(stage.Id);
            return View(fullStage);
        }



        [HttpPost("{stageSlug}/LearnNext")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LearnNext(string stageSlug)
        {
            var stage = await GetStageSlug(stageSlug);
            if (stage == null) return NotFound();

            await _progressService.MarkLearnCompletedAsync(GetUserId(), stage.Id);

            return RedirectToAction(nameof(Quiz), new { stageSlug });
        }



        [HttpGet("{stageSlug}/Quiz")]
        public async Task<IActionResult> Quiz(string stageSlug)
        {
            var stage = await GetStageSlug(stageSlug);
            if (stage == null) return NotFound();

            var deny = await CheckAccess(stage.Id);
            if (deny != null) return deny;

            var fullStage = await _stageRepo.GetWithQuizzesAsync(stage.Id);

            ViewBag.StageSlug = stageSlug;

            return View(fullStage.Quizzes);
        }



        [HttpPost("{stageSlug}/SubmitQuiz")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitQuiz(string stageSlug, [FromForm] Dictionary<int, string>? answers)
        {
            var stage = await GetStageSlug(stageSlug);
            if (stage == null) return NotFound();

            var userId = GetUserId();

            if (!await _progressService.CanAccessStageAsync(userId, stage.Id)) return Forbid();
            if (await _progressService.IsStageCompletedAsync(userId, stage.Id)) return BadRequest("این مرحله قبلا کامل شده و امکان ثبت دوباره وجود ندارد");

            await _progressService.MarkQuizCompletedAsync(userId, stage.Id, answers!);

            return RedirectToAction(nameof(Start));
        }



        [HttpGet(nameof(Completed))]
        public async Task<IActionResult> Completed()
        {
            var userId = GetUserId();
            var hasAnyQuizAttempt = await _certificateService.HasAnyQuizAttemptAsync(userId);
            if (!hasAnyQuizAttempt) return RedirectToAction(nameof(Start));

            var totalQuizzes = await _certificateService.GetTotalQuizCountAsync();
            var correctAnswers = await _certificateService.GetUserQuizScoreAsync(userId);
            var scorePercent = totalQuizzes > 0 ? (correctAnswers * 100 / totalQuizzes) : 0;

            if (scorePercent > 70)
            {
                var cert = await _certificateService.TryIssueCertificateAsync(userId);

                if (cert != null)
                {
                    return View(cert);
                } else
                {
                    ViewBag.Error = "مدرک هنوز صادر نشده است";
                    return View();
                }
            }

            ViewBag.CanRetry = true;
            ViewBag.ScorePercent = scorePercent;

            return View((Certificate?)null);
        }
    }
}