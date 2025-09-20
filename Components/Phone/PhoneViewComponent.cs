using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace vipkuryen.Components
{
    public class PhoneViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public PhoneViewComponent(AppDbContext context)
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
