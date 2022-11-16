using AutoMapper;
using JobSeeker.DbContexts;
using JobSeeker.Models;
using JobSeeker.Models.Dto;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace JobSeeker.Repository.JobRepository
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;

        public JobRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _db = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<JobDto> CreateUpdateJob(JobDto jobDto)
        {
            Job job = _mapper.Map<JobDto, Job>(jobDto);
            var jobDetail = await _db.Jobs.Where(u => u.Id == jobDto.Id).FirstOrDefaultAsync();
            try
            {
                if (jobDetail != null && jobDetail.Id > 0)
                {
                    job.ModifiedDate = DateTime.Now;
                    job.IsDelete = false;
                    job.IsActive = true;
                    _db.Update(job);
                }
                else
                {
                    jobDto.EntryDate = DateTime.Now;
                    jobDto.ModifiedDate = DateTime.Now;
                    jobDto.IsDelete = false;
                    jobDto.IsActive = true;
                    _db.Jobs.Add(job);
                }
                await _db.SaveChangesAsync();
                jobDto.Result = ResultStatus.Success;
                return jobDto;

            }
            catch (Exception ex)
            {
                jobDto.Result = ResultStatus.Failed;
            }
            return jobDto;
        }
        public async Task<bool> DeleteJob(int jobId)
        {
            try
            {
                Job job = await _db.Jobs.FirstOrDefaultAsync(x => x.Id == jobId);
                if (job == null)
                {
                    return false;
                }
                _db.Jobs.Remove(job);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<JobDto> GetJob(int jobId)
        {
            Job job = await _db.Jobs.Where(x => x.Id == jobId).FirstOrDefaultAsync();
            return _mapper.Map<JobDto>(job);
        }

        public async Task<IEnumerable<JobDto>> GetJobs(string email)
        {
            List<Job> jobs = new List<Job>();
            JobDto jobDto = new JobDto();
            jobs = await _db.Jobs.Where(x => x.Email == email).ToListAsync();
            return _mapper.Map<IEnumerable<JobDto>>(jobs);
        }

        public async Task<bool> CheckJob(int jobId)
        {
            var job = _db.Jobs.Where(x => x.Id == jobId).FirstOrDefaultAsync();
            if (job != null && job.Id == jobId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
