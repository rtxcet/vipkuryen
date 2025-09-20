using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using vipkuryen.Models;

namespace vipkuryen.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Default", new { area = "Admin" });
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                ModelState.AddModelError("", "Kullanıcı adı boş bırakılamaz.");
                return View();
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Şifre boş bırakılamaz.");
                return View();
            }

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Default", new { area = "Admin" });
            }
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Hesabınız kilitlenmiştir. Lütfen daha sonra tekrar deneyin.");
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError("", "Giriş yapmak için e-posta doğrulaması yapmalısınız.");
            }
            else
            {
                ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login", new { area = "Admin" }); // Admin login sayfasına yönlendir
        }
    }
}
