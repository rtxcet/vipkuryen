using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace vipkuryen.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var contact = await _context.Contacts
                                    .OrderByDescending(c => c.Id) // Id'ye göre ters sırala
                                    .ToListAsync();


            ViewBag.Message = TempData["SuccessMessage"];

            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
                return NotFound();


            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Başarıyla silindi.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null)
            {
                return NotFound(); // id bulunamazsa 404 dön
            }

            ViewBag.Message = TempData["SuccessMessage"];
            return View(contact); // Tek bir Contact nesnesi
        }
    }
}
