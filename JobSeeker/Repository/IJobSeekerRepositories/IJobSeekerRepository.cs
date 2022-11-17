using JobSeeker.Models;
using JobSeeker.Models.Dto;

namespace JobSeeker.Repository.IJobSeekerRepositories
{
    public interface IJobSeekerRepository
    {
        Task<RegistrationDto> CreateNewUser(RegistrationDto registrationDto);
        Task<RegistrationDto> UpdateProfileUser(RegistrationDto registrationDto);
        Task<RegistrationDto> Login(RegistrationDto registrationDto);
        Task<RegistrationDto> UpdatePhoto(RegistrationDto registrationDto);
        Task<RegistrationDto> GetJobSeeker(RegistrationDto registrationDto);
        Task<SkillDto> AddSkills(SkillDto skillDto);
        Task<IEnumerable<SkillDto>> GetSkills(int userId);
        Task<IEnumerable<JobDto>> GetJobs(JobDto jobDto);
        Task<IEnumerable<JobDto>> GetAllJobs();
    }
}
