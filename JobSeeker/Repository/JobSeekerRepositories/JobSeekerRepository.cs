using AutoMapper;
using JobSeeker.DbContexts;
using JobSeeker.Models;
using JobSeeker.Models.Dto;
using JobSeeker.Repository.IJobSeekerRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace JobSeeker.Repository.JobSeekerRepositories
{
    public class JobSeekerRepository : IJobSeekerRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;

        public JobSeekerRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _db = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<RegistrationDto> CreateNewUser(RegistrationDto registrationDto)
        {
            Registration registration = _mapper.Map<RegistrationDto, Registration>(registrationDto);
            registration.EntryDate = DateTime.Now;
            registration.ModifiedDate= DateTime.Now;
            registration.IsDelete = false;
            registration.IsActive = true;
            registration.IsLocked = false;
            var user =await _db.Registrations.Where(u => u.Email == registrationDto.Email).FirstOrDefaultAsync();
            try
            {
                if (user != null && user.Id > 0)
                {
                    registrationDto.Result = ResultStatus.AlreadyExit;
                }
                else
                {
                    _db.Registrations.Add(registration);
                    await _db.SaveChangesAsync();
                    registrationDto.Result = ResultStatus.Success;
                }
                
            }
            catch(Exception ex)
            {
                registrationDto.Result = ResultStatus.Failed;
            }
            return registrationDto;
        }

        public async Task<RegistrationDto> Login(RegistrationDto registrationDto)
        {
            Registration register = await _db.Registrations.Where(x => x.Email == registrationDto.Email && x.Password==registrationDto.Password).FirstOrDefaultAsync();
            return _mapper.Map<RegistrationDto>(register);
        }
    }
}
