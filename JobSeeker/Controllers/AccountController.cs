using JobSeeker.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace JobSeeker.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Dashboard()
        {
            RegistrationDto registrationDto= new RegistrationDto();
            registrationDto.Email = this.HttpContext.Session.GetString("Email");
            return View(registrationDto);
        }

        public async Task<IActionResult> LogOff()
        {
            this.HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }
    }
}
