using Microsoft.AspNetCore.Mvc;
using vipkuryen.Models;

namespace vipkuryen.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(new Contact());
        }


        [HttpPost]
        public IActionResult Index(Contact model)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(model);
                _context.SaveChanges();

                // Kullanıcıya mesaj göstermek için TempData kullanıyoruz
                TempData["Success"] = "Mesajınız başarıyla gönderildi!";
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
