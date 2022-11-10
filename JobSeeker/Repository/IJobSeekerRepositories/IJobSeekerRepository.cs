using JobSeeker.Models.Dto;

namespace JobSeeker.Repository.IJobSeekerRepositories
{
    public interface IJobSeekerRepository
    {
         Task<RegistrationDto> CreateNewUser(RegistrationDto registrationDto);
        Task<RegistrationDto> Login(RegistrationDto registrationDto);
    }
}
