using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vipkuryen.Models;

namespace vipkuryen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class DefaultController : Controller
    {

        private readonly AppDbContext _context;

        public DefaultController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var home = await _context.Homes.FirstOrDefaultAsync();

            if (home == null)
            {
                // Yeni bir Home nesnesi oluştur
                home = new Home
                {
                    Title = "",  // Dilersen boş bırakabilirsin
                    WhattsapPhone = "",
                    Phone = "",
                    Footer = ""
                };

                _context.Homes.Add(home);
                await _context.SaveChangesAsync();
            }

            // TempData'dan gelen mesajı al ve ViewBag'e ata
            ViewBag.Message = TempData["SuccessMessage"];

            return View(home);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Home model)
        {
            var home = await _context.Homes.FirstOrDefaultAsync();

            if (home != null)
            {
                home.Title = model.Title;
                home.WhattsapPhone = model.WhattsapPhone;
                home.Phone = model.Phone;
                home.Footer = model.Footer;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Başarıyla güncellendi.";
            }

            return RedirectToAction("Index");
        }
    }
}
