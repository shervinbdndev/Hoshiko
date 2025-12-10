using Hoshiko.Core.Interfaces;
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

        public StageController(IStageRepository stageRepo, IStageProgressService progressService)
        {
            _stageRepo = stageRepo;
            _progressService = progressService;
        }


        private string GetUserId() => User.Identity!.Name!;
        


        private async Task<IActionResult?> CheckAccess(int stageId)
        {
            var userId = GetUserId();

            if (!await _progressService.CanAccessStageAsync(userId, stageId)) return Forbid();

            return null;
        }



        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var stages = await _stageRepo.GetAllAsync();
            return View(stages);
        }



        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (_stageRepo is StageRepository concreteRepo)
            {
                var stage = await concreteRepo.GetWithQuizzesAsync(id);
                if (stage == null) return NotFound();

                return View(stage);
            }

            var s = await _stageRepo.GetByIdAsync(id);
            if (s == null) return NotFound();

            return View(s);
        }



        [HttpGet(nameof(Start))]
        public async Task<IActionResult> Start()
        {
            var userId = GetUserId();
            var nextStage = await _progressService.GetNextStageForUserAsync(userId);

            if (nextStage == null) return View("Completed");

            return RedirectToAction(nameof(Learn), new { stageId = nextStage.Id });
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
        public async Task<IActionResult> SubmitQuiz(int stageId, [FromForm] Dictionary<int, string> answers)
        {
            await _progressService.MarkQuizCompletedAsync(GetUserId(), stageId, answers);

            return RedirectToAction(nameof(Start));
        }
    }
}