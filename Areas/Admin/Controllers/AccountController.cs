using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using vipkuryen.Models;

namespace vipkuryen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var model = new ChangeUsernameViewModel
            {
                NewUsername = user.UserName
            };

            ViewBag.Message = TempData["SuccessMessage"];
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Index(ChangeUsernameViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // Aynı kullanıcı adı kullanılmasın diye kontrol
            var existingUser = await _userManager.FindByNameAsync(model.NewUsername);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                ModelState.AddModelError(string.Empty, "Bu kullanıcı adı zaten kullanılıyor.");
                return View(model);
            }

            user.UserName = model.NewUsername;
            user.NormalizedUserName = model.NewUsername.ToUpper();

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Kullanıcı adı başarıyla güncellendi.";
                return RedirectToAction("Index", "Account");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }


        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (changePasswordResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifre başarıyla değiştirildi.";
                return RedirectToAction("ChangePassword");
            }

            foreach (var error in changePasswordResult.Errors)
            {
                // İngilizce mesaj yerine kendin Türkçe yazabilirsin
                if (error.Code == "PasswordMismatch")
                {
                    ModelState.AddModelError(string.Empty, "Mevcut şifre yanlış.");
                }
                else if (error.Code == "PasswordTooShort")
                {
                    ModelState.AddModelError(string.Empty, "Şifre çok kısa.");
                }
                else if (error.Code == "PasswordRequiresNonAlphanumeric")
                {
                    ModelState.AddModelError(string.Empty, "Şifre en az bir özel karakter içermelidir.");
                }
                else
                {
                    // Diğer tüm hata mesajlarını olduğu gibi ekle
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


    }
}
