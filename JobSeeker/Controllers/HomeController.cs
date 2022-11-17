using JobSeeker.Models;
using JobSeeker.Models.Dto;
using JobSeeker.Repository.IJobSeekerRepositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

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
            RegistrationDto registrationDto= new RegistrationDto();
            registrationDto.UserTypes = BindUserType();
            return View(registrationDto);
        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationDto model)
        {
            if (ModelState.IsValid)
            {   
                var result = await _repository.CreateNewUser(model);
                ViewBag.Message = result.Result;
                ModelState.Clear();
            }           
            model.UserTypes = BindUserType();
            return View(model);
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
                HttpContext.Session.SetInt32("UserId", Convert.ToInt32(user.Id));

                if (user.ProfileImage != null)
                {
                    HttpContext.Session.SetString("ProfileImage", user.ProfileImage);
                }
                else
                {
                    HttpContext.Session.SetString("ProfileImage", SD.BlankImagePath);
                }

                //Check the user name and password  
                //Here can be implemented checking logic from the database  
                ClaimsIdentity identity = null;
                bool isAuthenticated = false;
                identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.UserType.Value.ToString())
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                isAuthenticated = true;

                if (isAuthenticated)
                {
                    var principal = new ClaimsPrincipal(identity);

                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    //return RedirectToAction("Index", "Home");
                }

                if (user.UserType ==UserTypeEnum.Admin)
                {
                    return RedirectToAction("Index", "AdminUser");
                }
                if (user.UserType == UserTypeEnum.JobSeeker)
                {
                    return RedirectToAction("Dashboard", "Account");
                }
                if (user.UserType == UserTypeEnum.Company)
                {
                    return RedirectToAction("Dashboard", "CompanyAccount");
                }


            }
            else
            {
                ViewBag.Message = ResultStatus.InvalidCredntial;
            }
            return View();
        }
        [HttpGet]
        [Route("browse-jobs")]
        public async Task<IActionResult> JobList()
        {
            JobDto jobDto = new JobDto();            
            var jobs = await _repository.GetAllJobs();
            jobDto.JobList = jobs.ToList();
            return View(jobDto);
        }

        [HttpGet]
        [Route("job-page")]
        public async Task<IActionResult> JobDetail()
        {
            JobDto jobDto = new JobDto();
            var jobs = await _repository.GetAllJobs();
            jobDto.JobList = jobs.ToList();
            return View(jobDto);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public List<UserType> BindUserType()
        {
            List<UserType> users = new List<UserType>();
            users.Add(new UserType() { Id = 1, Type = "Admin" });
            users.Add(new UserType() { Id = 2, Type = "JobSeeker" });
            users.Add(new UserType() { Id = 3, Type = "Company" });
            return users;
        }
    }
}