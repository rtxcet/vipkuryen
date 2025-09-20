using Microsoft.AspNetCore.Mvc;

namespace vipkuryen.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
