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

namespace JobSeeker.Controllers
{
    public class AdminUserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IJobRepository _jobRepository;
        public AdminUserController(ILogger<HomeController> logger, IJobRepository jobRepository)
        {
            _logger = logger;
            _jobRepository = jobRepository;
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

        public async Task<IActionResult> Dashboard()
        {
            return View();
        }
        public async Task<IActionResult> Profile()
        {
            return View();
        }
        private async Task<bool> JobDtoExists(int id)
        {
            return await _jobRepository.CheckJob(id);
        }
    }
}
