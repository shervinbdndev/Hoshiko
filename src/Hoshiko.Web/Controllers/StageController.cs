using Hoshiko.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hoshiko.Infrastructure.Repositories;

namespace Hoshiko.Web.Controllers
{
    
    [Authorize]
    public class StageController : Controller
    {
        private readonly GenericRepository<Stage> _stageRepo;

        public StageController(GenericRepository<Stage> stageRepo)
        {
            _stageRepo = stageRepo;
        }



    }
}