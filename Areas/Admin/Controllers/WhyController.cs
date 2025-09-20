using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vipkuryen.Models;

namespace vipkuryen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class WhyController : Controller
    {
        private readonly AppDbContext _context;

        public WhyController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var why = await _context.Whys.ToListAsync();

            ViewBag.Message = TempData["SuccessMessage"];
            return View(why);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Why why)
        {
            if (ModelState.IsValid)
            {
                // Veritabanındaki toplam hizmet sayısını kontrol et
                var whyCount = await _context.Whys.CountAsync();
                if (whyCount >= 3)
                {
                    TempData["SuccessMessage"] = "En fazla 3 Neden ekleyebilirsiniz.";
                    return RedirectToAction("Index");
                }

                _context.Whys.Add(why);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Neden başarıyla eklendi.";
                return RedirectToAction("Index");
            }

            return View(why);
        }


        public IActionResult Edit(int id)
        {
            var why = _context.Whys.FirstOrDefault(s => s.Id == id);
            if (why == null)
            {
                return NotFound();
            }
            return View(why);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Why why)
        {
            if (ModelState.IsValid)
            {
                var existingWhy = await _context.Whys.FindAsync(why.Id);
                if (existingWhy == null)
                {
                    return NotFound();
                }

                // Alanları güncelle
                existingWhy.Ikon = why.Ikon;
                existingWhy.Title = why.Title;
                existingWhy.Description = why.Description;

                _context.Update(existingWhy);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Neden başarıyla güncellendi.";
                return RedirectToAction("Index");
            }

            return View(why);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var why = await _context.Whys.FindAsync(id);
            if (why == null)
            {
                return NotFound();
            }

            _context.Whys.Remove(why);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Neden başarıyla silindi.";
            return RedirectToAction("Index");
        }


    }
}
