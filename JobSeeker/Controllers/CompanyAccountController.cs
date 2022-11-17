using JobSeeker.Models.Dto;
using JobSeeker.Repository.IJobSeekerRepositories;
using JobSeeker.Repository.JobRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Data;

namespace JobSeeker.Controllers
{
    [Authorize(Roles = "Company")]
    public class CompanyAccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IJobRepository _jobRepository;
        private IJobSeekerRepository _repository;
        public CompanyAccountController(ILogger<HomeController> logger, IJobRepository jobRepository, IJobSeekerRepository repository)
        {
            _logger = logger;
            _jobRepository = jobRepository;
            _repository = repository;
        }      
        public IActionResult Dashboard()
        {
            return View();
        }
        public async Task<IActionResult> LogOff()
        {
            this.HttpContext.Session.Clear();
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> ProfileImage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProfileImage(IFormFile formFile)
        {
            try
            {
                if (formFile != null && formFile.Length > 0)
                {
                    string profileImageName = formFile.FileName;
                    profileImageName = Path.GetFileName(profileImageName);
                    Guid guid = Guid.NewGuid();
                    string newfileName = guid.ToString();
                    string fileExtention = Path.GetExtension(formFile.FileName);

                    if (formFile.ContentType != "image/png" && formFile.ContentType != "image/jpeg")
                    {
                        ViewBag.Message = ResultStatus.InvalidFileType;
                        return View();
                    }

                    if (formFile.Length > 1000000)
                    {
                        ViewBag.Message = ResultStatus.MaxExceedFileSize;
                        return View();
                    }

                    profileImageName = newfileName + fileExtention;

                    RegistrationDto registrationDto = new RegistrationDto();
                    registrationDto.Email = this.HttpContext.Session.GetString("Email");
                    registrationDto.ProfileImage = SD.ProfileImagePath + profileImageName;
                    //Upload image path in registration table
                    var result = await _repository.UpdatePhoto(registrationDto);
                    if (result.Result == ResultStatus.Success)
                    {
                        // Save file in wwwroot folder
                        string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), SD.ProfileImageRootPath, profileImageName);
                        var stream = new FileStream(uploadpath, FileMode.Create);
                        formFile.CopyToAsync(stream);
                        ViewBag.Message = ResultStatus.Success;
                        var userAfterPhotoUploaded = await _repository.GetJobSeeker(registrationDto);
                        if (userAfterPhotoUploaded != null && userAfterPhotoUploaded.Id > 0)
                        {
                            HttpContext.Session.SetString("ProfileImage", userAfterPhotoUploaded.ProfileImage);
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Photo does not upload successfully. Try again";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = ResultStatus.IsNull;
                    return View();
                }
            }
            catch
            {
                ViewBag.Message = "Error while uploading the profile photo.";
                return View();
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {

            RegistrationDto registrationDto = new RegistrationDto();
            registrationDto.Email = this.HttpContext.Session.GetString("Email");
            registrationDto = await _repository.GetJobSeeker(registrationDto);
            return View(registrationDto);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(string email)
        {
            return RedirectToAction(nameof(ProfileEdit));
        }
        [HttpGet]
        public async Task<IActionResult> ProfileEdit()
        {

            RegistrationDto registrationDto = new RegistrationDto();
            registrationDto.Email = this.HttpContext.Session.GetString("Email");
            registrationDto = await _repository.GetJobSeeker(registrationDto);
            return View(registrationDto);
        }
        [HttpPost]
        public async Task<IActionResult> ProfileEdit(RegistrationDto registrationDto)
        {
            registrationDto.Email = this.HttpContext.Session.GetString("Email");
            registrationDto = await _repository.UpdateProfileUser(registrationDto);
            ViewBag.Message = registrationDto.Result;
            return View(registrationDto);
        }
    }
}
