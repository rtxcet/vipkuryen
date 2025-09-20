using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vipkuryen.Models;

namespace vipkuryen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.Services.ToListAsync();

            ViewBag.Message = TempData["SuccessMessage"];
            return View(services);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Service service)
        {
            if (ModelState.IsValid)
            {
                // Veritabanındaki toplam hizmet sayısını kontrol et
                var serviceCount = await _context.Services.CountAsync();
                if (serviceCount >= 3)
                {
                    TempData["SuccessMessage"] = "En fazla 3 hizmet ekleyebilirsiniz.";
                    return RedirectToAction("Index");
                }

                _context.Services.Add(service);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Servis başarıyla eklendi.";
                return RedirectToAction("Index");
            }

            return View(service);
        }


        public IActionResult Edit(int id)
        {
            var service = _context.Services.FirstOrDefault(s => s.Id == id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Service service)
        {
            if (ModelState.IsValid)
            {
                var existingService = await _context.Services.FindAsync(service.Id);
                if (existingService == null)
                {
                    return NotFound();
                }

                // Alanları güncelle
                existingService.Ikon = service.Ikon;
                existingService.Title = service.Title;
                existingService.Description = service.Description;

                _context.Update(existingService);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Hizmet başarıyla güncellendi.";
                return RedirectToAction("Index");
            }

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Hizmet başarıyla silindi.";
            return RedirectToAction("Index");
        }


    }
}
