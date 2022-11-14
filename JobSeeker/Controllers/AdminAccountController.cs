using Microsoft.AspNetCore.Mvc;

namespace JobSeeker.Controllers
{
    public class AdminAccountController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
