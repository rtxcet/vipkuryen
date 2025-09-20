using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace vipkuryen.Components
{
    public class HeroViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public HeroViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var siteSettings = await _context.Heros.FirstOrDefaultAsync();
            var about = await _context.Abouts.FirstOrDefaultAsync();

            ViewData["AboutSlug"] = about?.Title ?? "hakkımda";

            return View(siteSettings);
        }
    }
}
