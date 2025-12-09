using Hoshiko.Web.Models;
using Hoshiko.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hoshiko.Web.Controllers
{

    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;


        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }



        [HttpGet(nameof(Register))]
        public IActionResult Register() => View();


        [HttpGet(nameof(Login))]
        public IActionResult Login() => View();



        [HttpPost(nameof(Register))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _authService.RegisterAsync(
                model.UserName,
                model.Password,
                model.ConfirmPassword,
                model.FirstName,
                model.LastName
            );
            if (result) return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Registration Failed");

            return View(model);
        }



        [HttpPost(nameof(Login))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _authService.LoginAsync(model.UserName, model.Password);
            if (result) return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Invalid login attempt");

            return View(model);
        }



        [HttpPost(nameof(Logout))]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}