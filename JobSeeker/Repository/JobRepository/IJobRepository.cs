using JobSeeker.Models;
using JobSeeker.Models.Dto;

namespace JobSeeker.Repository.JobRepository
{
    public interface IJobRepository
    {
        Task<JobDto> CreateUpdateJob(JobDto jobDto);
        Task<JobDto> GetJob(int jobId);
        Task<IEnumerable<JobDto>> GetJobs(string email);
        Task <bool> DeleteJob(int jobId);
        Task<bool> CheckJob(int jobId);
    }
}
