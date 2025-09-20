using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vipkuryen.Models;

namespace vipkuryen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;

        public AboutController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var about = await _context.Abouts.FirstOrDefaultAsync();

            if (about == null)
            {
                // Yeni bir Home nesnesi oluştur
                about = new About
                {
                    Title = "",
                    Description = ""
                };

                _context.Abouts.Add(about);
                await _context.SaveChangesAsync();
            }

            // TempData'dan gelen mesajı al ve ViewBag'e ata
            ViewBag.Message = TempData["SuccessMessage"];

            return View(about);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(About model)
        {
            var about = await _context.Abouts.FirstOrDefaultAsync();

            if (about != null)
            {
                about.Title = model.Title;
                about.Description = model.Description;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Başarıyla güncellendi.";
            }

            return RedirectToAction("Index");
        }
    }
}
