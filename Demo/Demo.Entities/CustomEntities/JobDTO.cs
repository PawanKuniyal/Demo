namespace Helpa.Entities.CustomEntities
{
    using System.Collections.Generic;

    public class JobDTO
    {
        public string UserId { get; set; }

        public string JobTitle { get; set; }

        public string JobDescription { get; set; }

        public string Status { get; set; }

        public List<JobServices> JobServices { get; set; }

        public string HelperType { get; set; }

        public string JobType { get; set; }

        public string LocationName { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public List<JobScopes> JobScope { get; set; }

        public List<Receiver> Receivers { get; set; }

        public bool IsSunday { get; set; }

        public bool IsMonday { get; set; }

        public bool IsTuesday { get; set; }

        public bool IsWednesday { get; set; }

        public bool IsThursday { get; set; }

        public bool IsFriday { get; set; }

        public bool IsSaturday { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public bool IsHourly { get; set; }

        public bool IsDaily { get; set; }

        public bool IsMonthly { get; set; }

        public string MinPrice { get; set; }

        public string MaxPrice { get; set; }
    }

    public class JobServices
    {
        public string ServiceId { get; set; }
    }

    public class JobScopes
    {
        public string ScopeId { get; set; }
    }

    public class Receiver
    {
        public string ReceiverGender { get; set; }

        public string ReceiverAge { get; set; }
    }
}
