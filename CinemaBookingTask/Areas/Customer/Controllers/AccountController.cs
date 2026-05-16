using CinemaBookingTask.Models;
using CinemaBookingTask.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBookingTask.Controllers
{
    [Area("Customer")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            ApplicationUser user = new()
            {
                FName = vm.FName,
                LName = vm.LName,
                Email = vm.Email,
                UserName = vm.UserName
            };

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(vm);
            }

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user =
                await _userManager.FindByEmailAsync(vm.EmailOrUserName)
                ?? await _userManager.FindByNameAsync(vm.EmailOrUserName);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Login");

                return View(vm);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                vm.Password,
                vm.RememberMe,
                false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid Login");

                return View(vm);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user =
                await _userManager.FindByEmailAsync(vm.EmailOrUserName)
                ?? await _userManager.FindByNameAsync(vm.EmailOrUserName);

            if (user == null)
            {
                ModelState.AddModelError("", "User Not Found");
                return View(vm);
            }

            return RedirectToAction("Login");
        }
    }
}