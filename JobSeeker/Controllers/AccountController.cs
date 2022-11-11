﻿using JobSeeker.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using JobSeeker.Repository.IJobSeekerRepositories;
using System.ComponentModel.DataAnnotations;

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
        
        [HttpPost]        
        public async Task<IActionResult> ProfileImage(IFormFile formFile)
        {            
            try
            {
                if (formFile!=null && formFile.Length > 0)
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
    }
}
