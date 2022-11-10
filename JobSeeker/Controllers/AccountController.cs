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
    }
}
