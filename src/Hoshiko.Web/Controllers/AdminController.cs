using Hoshiko.Core.Interfaces;
using Hoshiko.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Hoshiko.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Hoshiko.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Hoshiko.Web.Services;
using Hoshiko.Web.Models;

namespace Hoshiko.Web
{

    [Authorize(Roles = "Admin")]
    [Route("Admin/[action]")]
    public class AdminController : Controller
    {

        private readonly UserService _userService;
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<Quiz> _quizRepo;
        private readonly IGenericRepository<Stage> _stageRepo;
        private readonly IGenericRepository<Learn> _learnRepo;
        private readonly ICertificateService _certificateService;
        private readonly IStageProgressService _stageProgressService;
        
        public AdminController(
            UserService userService,
            ApplicationDbContext context,
            IGenericRepository<Quiz> quizRepo,
            IGenericRepository<Stage> stageRepo,
            IGenericRepository<Learn> learnRepo,
            ICertificateService certificateService,
            IStageProgressService stageProgressService
        )
        {
            _context = context;
            _quizRepo = quizRepo;
            _stageRepo = stageRepo;
            _learnRepo = learnRepo;
            _userService = userService;
            _certificateService = certificateService;
            _stageProgressService = stageProgressService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var stages = await _context.Stages
                .Include(s => s.Learns)
                .Include(s => s.Quizzes)
                .ToListAsync();

            return View("Admin", stages);
        }


        public async Task<IActionResult> Users()
        {
            var users = await _userService.GetAllUsersAsync();
            var stages = await _stageProgressService.GetAllStagesAsync();

            var userProgressList = new List<UserProgressViewModel>();

            foreach (var user in users)
            {
                var progressPerStage = new List<StageProgressViewModel>();

                foreach (var stage in stages)
                {
                    var progress = await _context.UserStageProgresses
                                                .FirstOrDefaultAsync(p => p.UserId == user.Id && p.StageId == stage.Id);

                    progressPerStage.Add(new StageProgressViewModel
                    {
                        StageName = stage.Name,
                        IsLearnCompleted = progress?.IsLearnCompleted ?? false,
                        IsQuizCompleted = progress?.IsQuizCompleted ?? false
                    });
                }

                var certificate = await _certificateService.GetUserCertificateAsync(user.Id);

                userProgressList.Add(new UserProgressViewModel
                {
                    User = user,
                    StagesProgress = progressPerStage,
                    CertificateCode = certificate?.CertificateCode
                });
            }

            return View(userProgressList);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStage(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest();

            var slug = SlugHelper.GenerateSlug(name);

            var exists = await _context.Stages.AnyAsync(s => s.Slug == slug);
            if (exists) slug += "-" + Guid.NewGuid().ToString("N")[..6];

            await _stageRepo.AddAsync(new Stage
            {
                Name = name,
                Slug = slug
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStage(int id)
        {
            var stage = await _context.Stages
                .Include(s => s.Learns)
                .Include(s => s.Quizzes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stage == null) return NotFound();

            _context.Stages.Remove(stage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLearn(int stageId, string title, string content)
        {
            var slug = SlugHelper.GenerateSlug(title);

            var exists = await _context.Learns.AnyAsync(l => l.StageId == stageId && l.Slug == slug);
            if (exists) slug += "-" + Guid.NewGuid().ToString("N")[..6];

            await _learnRepo.AddAsync(new Learn
            {
                StageId = stageId,
                Title = title,
                Slug = slug,
                Content = content
            });

            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLearn(int id)
        {
            var learn = await _context.Learns.FindAsync(id);
            if (learn == null) return NotFound();

            _context.Learns.Remove(learn);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuiz(Quiz model)
        {
            await _quizRepo.AddAsync(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null) return NotFound();

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}