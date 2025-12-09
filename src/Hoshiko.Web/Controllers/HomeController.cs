using Microsoft.AspNetCore.Mvc;

namespace Hoshiko.Web.Controllers
{

    public class HomeController : Controller
    {

        [HttpGet("/")]
        public IActionResult Index() => View("~/Views/Index.cshtml");
    }
}