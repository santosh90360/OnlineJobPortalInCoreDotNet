using JobSeeker.Models;
using JobSeeker.Models.Dto;
using JobSeeker.Repository.IJobSeekerRepositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JobSeeker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IJobSeekerRepository _repository;

        public HomeController(ILogger<HomeController> logger, IJobSeekerRepository jobSeekerRepository)
        {
            _logger = logger;
            _repository = jobSeekerRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationDto model)
        {
            if (ModelState.IsValid)
            {
                model.UserType = "JobSeeker";
                var result = await _repository.CreateNewUser(model);
                ViewBag.Message = result.Result;
                ModelState.Clear();
            }
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(RegistrationDto registrationDto)
        {
            var user =await _repository.Login(registrationDto);
            if (user != null &&  user.Id > 0)
            {
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetString("Name", user.Name);
                HttpContext.Session.SetString("ProfileImage", user.ProfileImage);
             
                return RedirectToAction("Dashboard","Account");
            }
            else
            {
                ViewBag.Message = ResultStatus.InvalidCredntial;
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}