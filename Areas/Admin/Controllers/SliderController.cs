using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vipkuryen.Models;

namespace vipkuryen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;

        public SliderController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Sliders.ToListAsync();

            ViewBag.Message = TempData["SuccessMessage"];
            return View(products);
        }


        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Slider slider)
        {
            if (ModelState.IsValid)
            {
                // Eğer görsel yükleme varsa (opsiyonel olarak işle)
                if (slider.ImageFile != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/slider");

                    // Klasör yoksa oluştur
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(slider.ImageFile.FileName);
                    var path = Path.Combine(uploadsFolder, fileName);

                    // Dosyayı asenkron şekilde kaydet (await çok önemli!)
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await slider.ImageFile.CopyToAsync(stream);
                    }

                    slider.ImageUrl = "/uploads/slider/" + fileName;
                }


                _context.Sliders.Add(slider);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Slider Başarıyla eklendi.";
                return RedirectToAction("Index");
            }

            // Hatalıysa kategorileri tekrar gönder

            return View(slider);
        }



        // ProductController.cs - Silme ve düzenleme işlemleri

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
                return NotFound();

            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Slider slider)
        {
            if (id != slider.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var existingSlider = await _context.Sliders.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                if (existingSlider == null)
                    return NotFound();

                // Yeni görsel yüklendiyse
                if (slider.ImageFile != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/slider");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(slider.ImageFile.FileName);
                    var path = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await slider.ImageFile.CopyToAsync(stream);
                    }

                    // Eski resmi sil
                    if (!string.IsNullOrEmpty(existingSlider.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingSlider.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }

                    slider.ImageUrl = "/uploads/slider/" + fileName;
                }
                else
                {
                    slider.ImageUrl = existingSlider.ImageUrl; // Aynı kalacaksa
                }

                _context.Update(slider);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Banner başarıyla güncellendi.";
                return RedirectToAction("Index");
            }

            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
                return NotFound();

            // Resmi sil
            if (!string.IsNullOrEmpty(slider.ImageUrl))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", slider.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Banner başarıyla silindi.";
            return RedirectToAction("Index");
        }


    }
}
