using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobSeeker.DbContexts;
using JobSeeker.Models;
using JobSeeker.Repository.IJobSeekerRepositories;
using JobSeeker.Repository.JobRepository;
using JobSeeker.Models.Dto;
using NuGet.Protocol.Core.Types;

namespace JobSeeker.Controllers
{
    public class AdminUserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IJobRepository _jobRepository;
        private IJobSeekerRepository _repository;
        public AdminUserController(ILogger<HomeController> logger, IJobRepository jobRepository, IJobSeekerRepository repository)
        {
            _logger = logger;
            _jobRepository = jobRepository;
            _repository = repository;
        }
        // GET: AdminUser
        public async Task<IActionResult> Index()
        {
            string email = this.HttpContext.Session.GetString("Email");
            var jobs = await _jobRepository.GetJobs(email);
            return View(jobs);
        }

        // GET: AdminUser/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var jobDto = await _jobRepository.GetJob(Convert.ToInt32(id));
            if (jobDto == null)
            {
                return NotFound();
            }
            return View(jobDto);
        }

        // GET: AdminUser/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,JobTitle,Location,JobType,Category,JobTags,StartDate,ClosingDate,ExtendedDate,CompanyName,Website,JobDescription,TwitterUsername,Logo,IsActive,IsDelete,EntryDate,ModifiedDate,IPAddress")] JobDto jobDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _jobRepository.CreateUpdateJob(jobDto);
                if (result.Result == ResultStatus.Success)
                {
                    ViewBag.Message = result.Result;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Message = result.Result;
                    return View(jobDto);
                }
            }
            return View(jobDto);
        }

        // GET: AdminUser/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var jobDto = await _jobRepository.GetJob(Convert.ToInt32(id));
            if (jobDto == null)
            {
                return NotFound();
            }
            return View(jobDto);
        }

        // POST: AdminUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,JobTitle,Location,JobType,Category,JobTags,StartDate,ClosingDate,ExtendedDate,CompanyName,Website,JobDescription,TwitterUsername,Logo,IsActive,IsDelete,EntryDate,ModifiedDate,IPAddress")] JobDto jobDto)
        {
            if (id != jobDto.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                   _jobRepository.CreateUpdateJob(jobDto);                   
                }
                catch (Exception ex)
                {
                    if (!await JobDtoExists(jobDto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ex;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jobDto);
        }

        // GET: AdminUser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var jobDto = await _jobRepository.GetJob(Convert.ToInt32(id));
            if (jobDto == null)
            {
                return NotFound();
            }
            return View(jobDto);
        }

        // POST: AdminUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JobDto'  is null.");
            }
            var jobDto = await _jobRepository.DeleteJob(id);
            if (jobDto != null && jobDto==true)
            {
                ViewBag.Message = ResultStatus.Success;
            }           
            return RedirectToAction(nameof(Index));
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
        private async Task<bool> JobDtoExists(int id)
        {
            return await _jobRepository.CheckJob(id);
        }
    }
}
