using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace vipkuryen.Components
{
    public class HeadViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public HeadViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var siteSettings = await _context.Homes.FirstOrDefaultAsync();
            return View(siteSettings);
        }
    }
}
