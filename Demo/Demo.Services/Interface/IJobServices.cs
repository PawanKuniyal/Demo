using System.Linq;
using System.Threading.Tasks;
using Helpa.Entities;

namespace Helpa.Services.Interface
{
    public interface IJobServices
    {
        IQueryable<Job> GetAllJobs();
        IQueryable<Job> GetJobs(int Id);
        Job GetJob(int Id);
        Job AddJob(Job job);
        Task<Job> AddJobAsync(Job job);
        int UpdateJob(Job job);
        Task<int> UpdateJobAsync(Job job);
        int RemoveJob(Job job);
        Task<int> RemoveJobAsync(Job job);
        JobService AddJobService(JobService jobService);
        Task<JobService> AddJobServiceAsync(JobService jobService);
        Task<Job> GetJobAsync(int id);
    }
}
