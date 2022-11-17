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
            registration.ModifiedDate = DateTime.Now;
            registration.IsDelete = false;
            registration.IsActive = true;
            registration.IsLocked = false;
            var user = await _db.Registrations.Where(u => u.Email == registrationDto.Email).FirstOrDefaultAsync();
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
            catch (Exception ex)
            {
                registrationDto.Result = ResultStatus.Failed;
            }
            return registrationDto;
        }

        public async Task<RegistrationDto> UpdateProfileUser(RegistrationDto registrationDto)
        {
            Registration registration = _mapper.Map<RegistrationDto, Registration>(registrationDto);
            registration.ModifiedDate = DateTime.Now;
            registration.IsDelete = false;
            registration.IsActive = true;
            registration.IsLocked = false;
            var user = await _db.Registrations.Where(u => u.Email == registrationDto.Email).FirstOrDefaultAsync();
            try
            {
                if (user != null && user.Id > 0)
                {
                    user.ModifiedDate = DateTime.Now;
                    user.Mobile = registration.Mobile;
                    user.Gender = registration.Gender;
                    user.MaritalStatus = registration.MaritalStatus;
                    user.DOB = registration.DOB;
                    _db.Registrations.Update(user);
                    await _db.SaveChangesAsync();
                    registrationDto.Result = ResultStatus.Success;
                }
                else
                {
                    registrationDto.Result = ResultStatus.RecordNotFound;
                }

            }
            catch (Exception ex)
            {
                registrationDto.Result = ResultStatus.Failed;
            }
            return registrationDto;
        }
        public async Task<RegistrationDto> Login(RegistrationDto registrationDto)
        {
            Registration register = await _db.Registrations.Where(x => x.Email == registrationDto.Email && x.Password == registrationDto.Password).FirstOrDefaultAsync();
            return _mapper.Map<RegistrationDto>(register);
        }
        public async Task<RegistrationDto> UpdatePhoto(RegistrationDto registrationDto)
        {
            Registration registration = _mapper.Map<RegistrationDto, Registration>(registrationDto);
            registration.ModifiedDate = DateTime.Now;
            var user = await _db.Registrations.Where(u => u.Email == registrationDto.Email).FirstOrDefaultAsync();
            try
            {
                if (user.Id > 0)
                {
                    user.ProfileImage = registrationDto.ProfileImage;
                    _db.Registrations.Update(user);
                    await _db.SaveChangesAsync();
                    registrationDto.Result = ResultStatus.Success;
                }
                else
                {
                    registrationDto.Result = ResultStatus.Failed;
                }

            }
            catch (Exception ex)
            {
                registrationDto.Result = ResultStatus.Failed;
            }
            return registrationDto;
        }
        public async Task<RegistrationDto> GetJobSeeker(RegistrationDto registrationDto)
        {
            Registration register = await _db.Registrations.Where(x => x.Email == registrationDto.Email).FirstOrDefaultAsync();
            return _mapper.Map<RegistrationDto>(register);
        }
        public async Task<SkillDto> AddSkills(SkillDto skillDto)
        {
            Skill skill = _mapper.Map<SkillDto, Skill>(skillDto);
            skill.EntryDate = DateTime.Now;
            skill.ModifiedDate = DateTime.Now;
            var user = await _db.Skills.Where(u => u.Id == skillDto.Id && u.UserId == skillDto.UserId).FirstOrDefaultAsync();
            try
            {
                if (user != null && user.Id > 0)
                {
                    skillDto.Result = ResultStatus.AlreadyExit;
                }
                else
                {
                    _db.Skills.Add(skill);
                    await _db.SaveChangesAsync();
                    skillDto.Result = ResultStatus.Success;
                }

            }
            catch (Exception ex)
            {
                skillDto.Result = ResultStatus.Failed;
            }
            return skillDto;
        }
        public async Task<IEnumerable<SkillDto>> GetSkills(int userId)
        {
            List<Skill> skills = new List<Skill>();
            SkillDto skillDto = new SkillDto();
            skills = await _db.Skills.Where(x => x.UserId == userId).ToListAsync();
            return _mapper.Map<IEnumerable<SkillDto>>(skills);
        }

        #region "Job Detail"

        public async Task<IEnumerable<JobDto>> GetJobs(JobDto jobDto)
        {
            List<Job> job = await _db.Jobs.Where(x => x.Email.Contains(jobDto.Email) || 
            x.JobDescription.Contains(jobDto.JobDescription) || x.JobTitle.Contains(jobDto.JobTitle)).ToListAsync();
            return _mapper.Map<List<JobDto>>(job);
        }
        public async Task<IEnumerable<JobDto>> GetAllJobs()
        {
            List<Job> job = await _db.Jobs.ToListAsync();
            return _mapper.Map<List<JobDto>>(job);
        }


        #endregion   
    }
}
