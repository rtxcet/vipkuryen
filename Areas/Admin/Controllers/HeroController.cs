using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vipkuryen.Models;

namespace vipkuryen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class HeroController : Controller
    {

        private readonly AppDbContext _context;

        public HeroController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var hero = await _context.Heros.FirstOrDefaultAsync();

            if (hero == null)
            {
                // Yeni bir Home nesnesi oluştur
                hero = new Hero
                {
                    Title = "",  
                    Description = ""
                };

                _context.Heros.Add(hero);
                await _context.SaveChangesAsync();
            }

            // TempData'dan gelen mesajı al ve ViewBag'e ata
            ViewBag.Message = TempData["SuccessMessage"];

            return View(hero);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Hero model)
        {
            var hero = await _context.Heros.FirstOrDefaultAsync();

            if (hero != null)
            {
                hero.Title = model.Title;
                hero.Description = model.Description;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Başarıyla güncellendi.";
            }

            return RedirectToAction("Index");
        }
    }
}
