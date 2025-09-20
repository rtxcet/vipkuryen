using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace vipkuryen.Controllers
{
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
            return View(about);
        }
    }
}
