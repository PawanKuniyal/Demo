using System;
using System.Linq;
using System.Threading.Tasks;
using Helpa.Services.Interface;
using Helpa.Services.Repository;
using Helpa.Entities;

namespace Helpa.Services
{
    public class JobServices : IJobServices
    {
        private IRepository<Job> jobContext;
        private IRepository<JobService> jobServiceContext;
        private IRepository<JobScope> jobScopeContext;

        public JobServices()
        {
            jobContext = new Repository<Job>();
            jobServiceContext = new Repository<JobService>();
            jobScopeContext = new Repository<JobScope>();
        }

        #region Jobs
        public IQueryable<Job> GetAllJobs()
        {
            var result = jobContext.GetAll(x => x.RowStatus != "D").IncludeEntities(x => x.JobServices);
            return result;
        }

        public IQueryable<Job> GetJobs(int Id)
        {
            var result = jobContext.GetById(x => x.JobId == Id && x.RowStatus != "D")
                                   .IncludeEntities(x => x.JobLocation,
                                                    x => x.JobPrice,
                                                    x => x.JobServices,
                                                    x => x.Recievers,
                                                    x => x.JobTime,
                                                    x => x.JobLocation);
            return result;
        }

        public Job GetJob(int Id)
        {
            var result = jobContext.GetEntity(x => x.JobId == Id);
            return result;
        }

        public async Task<Job> GetJobAsync(int id)
        {
            var result = await jobContext.GetEntityAsync(x => x.JobId == id);
            return result;
        }

        public Job AddJob(Job job)
        {
            job.CreatedDate = DateTime.UtcNow;
            job.ExpiryDate = job.CreatedDate.AddDays(30);
            job.RowStatus = "I";
            job.Status = true;
            var result = jobContext.Insert(job);
            return result;
        }

        public async Task<Job> AddJobAsync(Job job)
        {
            job.CreatedDate = DateTime.UtcNow;
            job.ExpiryDate = job.CreatedDate.AddDays(30);
            job.RowStatus = "I";
            job.Status = true;
            var result = await jobContext.InsertAsync(job);
            return result;
        }

        public int UpdateJob(Job job)
        {
            job.UpdatedDate = DateTime.UtcNow;
            job.RowStatus = "U";
            var result = jobContext.Update(job);
            return result;
        }

        public async Task<int> UpdateJobAsync(Job job)
        {
            job.UpdatedDate = DateTime.UtcNow;
            job.RowStatus = "U";
            var result = await jobContext.UpdateAsync(job);
            return result;
        }

        public int RemoveJob(Job job)
        {
            job.UpdatedDate = DateTime.UtcNow;
            job.RowStatus = "D";
            var result = jobContext.Update(job);
            return result;
        }

        public async Task<int> RemoveJobAsync(Job job)
        {
            job.UpdatedDate = DateTime.UtcNow;
            job.RowStatus = "D";
            var result = await jobContext.UpdateAsync(job);
            return result;
        }
        #endregion

        #region Job Service
        public JobService AddJobService(JobService jobService)
        {
            jobService.CreatedDate = DateTime.UtcNow;
            jobService.RowStatus = "I";
            var result = jobServiceContext.Insert(jobService);
            return result;
        }

        public async Task<JobService> AddJobServiceAsync(JobService jobService)
        {
            jobService.CreatedDate = DateTime.UtcNow;
            jobService.RowStatus = "I";
            var result = await jobServiceContext.InsertAsync(jobService);
            return result;
        }
        #endregion

        #region Job Scope
        public JobScope AddJobScope(JobScope jobScope)
        {
            jobScope.CreatedDate = DateTime.UtcNow;
            jobScope.RowStatus = "I";
            var result = jobScopeContext.Insert(jobScope);
            return result;
        }
        #endregion
    }
}
