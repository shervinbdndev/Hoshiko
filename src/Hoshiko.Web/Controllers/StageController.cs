using System.Security.Claims;
using Hoshiko.Core.Interfaces;
using Hoshiko.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hoshiko.Infrastructure.Repositories;

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

            return RedirectToAction(nameof(Learn), new { stageId = nextStage.Id });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Retry()
        {
            var userId = GetUserId();

            await _progressService.ResetQuizProgressWithLimitAsync(userId);

            return RedirectToAction(nameof(Start));
        }




        [HttpGet("Learn/{stageId}")]
        public async Task<IActionResult> Learn(int stageId)
        {
            var deny = await CheckAccess(stageId);
            if (deny != null) return deny;

            var stage = await _stageRepo.GetWithQuizzesAsync(stageId);
            return stage == null ? NotFound() : View(stage);
        }



        [HttpPost("LearnNext/{stageId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LearnNext(int stageId)
        {
            await _progressService.MarkLearnCompletedAsync(GetUserId(), stageId);

            return RedirectToAction(nameof(Quiz), new { stageId });
        }



        [HttpGet("Quiz/{stageId}")]
        public async Task<IActionResult> Quiz(int stageId)
        {
            var deny = await CheckAccess(stageId);
            if (deny != null) return deny;

            var stage = await _stageRepo.GetWithQuizzesAsync(stageId);

            ViewBag.StageId = stageId;

            return stage == null ? NotFound() : View(stage.Quizzes);
        }



        [HttpPost("SubmitQuiz/{stageId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitQuiz(int stageId, [FromForm] Dictionary<int, string>? answers)
        {
            var userId = GetUserId();

            var stage = await _stageRepo.GetByIdAsync(stageId);
            if (stage == null) return NotFound();

            if (!await _progressService.CanAccessStageAsync(userId, stageId)) return Forbid();
            if (await _progressService.IsStageCompletedAsync(userId, stageId)) return BadRequest("این مرحله قبلا کامل شده و امکان ثبت دوباره وجود ندارد");

            await _progressService.MarkQuizCompletedAsync(userId, stageId, answers!);

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