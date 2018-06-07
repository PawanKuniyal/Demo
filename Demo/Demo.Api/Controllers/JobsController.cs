using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity.Spatial;
using Helpa.Entities;
using Helpa.Entities.Context;
using Helpa.Entities.CustomEntities;
using Helpa.Api.Utilities;
using Helpa.Services.Interface;

namespace Helpa.Api.Controllers
{
    public class JobsController : ApiController
    {
        private HelpaContext db = new HelpaContext();
        private IJobServices jobServices;

        public JobsController()
        {
            jobServices = new Services.JobServices();
        }

        // GET: api/Jobs
        public IQueryable<Job> GetJobs()
        {
            var result = jobServices.GetAllJobs();
            return result;
        }

        // GET: api/Jobs/5
        [ResponseType(typeof(Job))]
        public async Task<IHttpActionResult> GetJob(int id)
        {
            Job job = await jobServices.GetJobAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            return Ok(job);
        }

        // PUT: api/Jobs/5
        [Authorize(Roles = "PARENT")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutJob(int id, Job job)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != job.JobId)
            {
                return BadRequest();
            }

            db.Entry(job).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Jobs
        //[Authorize(Roles = "PARENT")]
        [ResponseType(typeof(Job))]
        public async Task<IHttpActionResult> PostJob([FromBody] JobDTO jobDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Job job = new Job();

            try
            {
                DbGeography geography = null;

                if (!String.IsNullOrEmpty(jobDTO.Latitude) && !String.IsNullOrEmpty(jobDTO.Longitude))
                {
                    double lat = Convert.ToDouble(jobDTO.Latitude);
                    double longi = Convert.ToDouble(jobDTO.Longitude);
                    geography = LocationPoint.CreatePoint(lat, longi);
                }
                job.JobLocation = new JobLocation();
                job.JobPrice = new JobPrice();
                job.JobTime = new JobTime();

                job.CreatedUserId = Convert.ToInt32(jobDTO.UserId);
                job.JobTiltle = jobDTO.JobTitle;
                job.HelperType = jobDTO.HelperType;
                job.JobDescription = jobDTO.JobDescription;
                job.JobLocation.CreatedDate = DateTime.UtcNow;
                job.JobLocation.JobLocationGeography = geography;
                job.JobLocation.JobLocationName = jobDTO.LocationName;
                job.JobLocation.RowStatus = "I";
                job.JobPrice.CreatedDate = DateTime.UtcNow;
                job.JobPrice.Daily = jobDTO.IsDaily;
                job.JobPrice.Hourly = jobDTO.IsHourly;
                job.JobPrice.MaxPrice = Convert.ToDecimal(jobDTO.MaxPrice);
                job.JobPrice.MinPrice = Convert.ToDecimal(jobDTO.MinPrice);
                job.JobPrice.Monthly = jobDTO.IsMonthly;
                job.JobPrice.RowStatus = "I";
                job.JobTime.CreatedDate = DateTime.UtcNow;
                job.JobTime.EndTime = TimeSpan.Parse(jobDTO.EndTime);
                job.JobTime.Friday = jobDTO.IsFriday;
                job.JobTime.Monday = jobDTO.IsMonday;
                job.JobTime.Saturday = jobDTO.IsSaturday;
                job.JobTime.StartTime = TimeSpan.Parse(jobDTO.StartTime);
                job.JobTime.Sunday = jobDTO.IsSunday;
                job.JobTime.Thursday = jobDTO.IsThursday;
                job.JobTime.Tuesday = jobDTO.IsTuesday;
                job.JobTime.Wednesday = jobDTO.IsWednesday;
                job.JobType = jobDTO.JobType;

                var result = await jobServices.AddJobAsync(job);
                int JobId = result.JobId;

                if (jobDTO.JobServices.Count > 0)
                {
                    JobService jobService = null;

                    foreach (var item in jobDTO.JobServices)
                    {
                        jobService = new JobService
                        {
                            JobId = JobId,
                            ServiceId = Convert.ToInt32(item.ServiceId)
                        };
                        var result1 = await jobServices.AddJobServiceAsync(jobService);
                        int JobServiceId = result1.JobServiceId;

                        if (JobServiceId > 0)
                        {
                            if (jobDTO.JobScope.Count > 0)
                            {
                                JobScope jobScope = null;

                                foreach (var scopeItem in jobDTO.JobScope)
                                {
                                    jobScope = new JobScope();
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return Ok(job.JobId);
        }

        // DELETE: api/Jobs/5
        [Authorize(Roles = "PARENT")]
        [ResponseType(typeof(Job))]
        public async Task<IHttpActionResult> DeleteJob(int id)
        {
            Job job = await db.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            db.Jobs.Remove(job);
            await db.SaveChangesAsync();

            return Ok(job);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JobExists(int id)
        {
            return db.Jobs.Count(e => e.JobId == id) > 0;
        }
    }
}