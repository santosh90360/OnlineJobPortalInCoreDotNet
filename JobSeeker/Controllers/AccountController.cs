using JobSeeker.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using JobSeeker.Repository.IJobSeekerRepositories;

namespace JobSeeker.Controllers
{
    public class AccountController : Controller
    {
        private IHostingEnvironment Environment;
        private IJobSeekerRepository _repository;

       
        public AccountController(IHostingEnvironment _environment, IJobSeekerRepository jobSeekerRepository)
        {
            Environment = _environment;
            _repository = jobSeekerRepository;
        }
        public IActionResult Dashboard()
        {
            RegistrationDto registrationDto = new RegistrationDto();
            registrationDto.Email = this.HttpContext.Session.GetString("Email");
            return View(registrationDto);
        }

        public async Task<IActionResult> LogOff()
        {
            this.HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> ProfileImage()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> ProfileImage(List<IFormFile> postedFiles)
        //{
        //    string wwwPath = this.Environment.WebRootPath;
        //    string contentPath = this.Environment.ContentRootPath;

        //    string path = Path.Combine(this.Environment.WebRootPath, @"images\profileimages");
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }

        //    List<string> uploadedFiles = new List<string>();
        //    foreach (IFormFile postedFile in postedFiles)
        //    {
        //        string fileName = Path.GetFileName(postedFile.FileName);
        //        using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
        //        {
        //            postedFile.CopyTo(stream);
        //            uploadedFiles.Add(fileName);
        //            ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
        //        }
        //    }

        //    return View();
        //}
        [HttpPost]        
        public async Task<IActionResult> ProfileImage(IFormFile formFile)
        {
            try
            {
                string fileName = formFile.FileName;
                fileName = Path.GetFileName(fileName);
                RegistrationDto registrationDto = new RegistrationDto();
                registrationDto.Email= this.HttpContext.Session.GetString("Email");
                registrationDto.ProfileImage = "/images/profileimages/" + fileName;
                var result=await _repository.UpdatePhoto(registrationDto);
                if (result.Result == ResultStatus.Success)
                {
                    string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\profileimages", fileName);
                    var stream = new FileStream(uploadpath, FileMode.Create);
                    formFile.CopyToAsync(stream);
                    ViewBag.Message = "Photo uploaded successfully.";
                    var userAfterPhotoUploaded= await _repository.GetJobSeeker(registrationDto);
                    if (userAfterPhotoUploaded != null && userAfterPhotoUploaded.Id > 0)
                    {                       
                        HttpContext.Session.SetString("ProfileImage", userAfterPhotoUploaded.ProfileImage);
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Photo does not upload successfully. Try again";
                }               
            }
            catch
            {
                ViewBag.Message = "Error while uploading the profile photo.";
            }
            return View();
        }
    }
}
