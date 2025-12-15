using System.Security.Claims;
using Hoshiko.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoshiko.Web.Controllers
{

    public class HomeController : Controller
    {

        private ICertificateService _certService;

        public HomeController(ICertificateService certService)
        {
            _certService = certService;
        }

        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            bool canGetCertificate = false;

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                canGetCertificate = await _certService.CanIssueCertificateAsync(userId);
            }

            ViewBag.CanGetCertificate = canGetCertificate;

            return View("~/Views/Index.cshtml");
        }



        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Search(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return NotFound();

            var cert = await _certService.GetByCertificateCodeAsync(code);
            if (cert == null) return NotFound();

            return PartialView("_Certificate", cert);
        }
    }
}