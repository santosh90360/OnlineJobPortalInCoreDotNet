using Microsoft.AspNetCore.Mvc;

namespace JobSeeker.Controllers
{
    public class CompanyAccountController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
