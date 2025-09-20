using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace vipkuryen.Components
{
    public class WhyViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public WhyViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var siteSettings = await _context.Homes.FirstOrDefaultAsync();
            var whies = await _context.Whys.ToListAsync();

            ViewData["SiteTitle"] = siteSettings?.Title ?? "VIP Kuryen";

            return View(whies);
        }

    }
}
