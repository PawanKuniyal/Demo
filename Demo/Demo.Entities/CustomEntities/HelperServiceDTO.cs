namespace Helpa.Entities.CustomEntities
{
    using System.Collections.Generic;

    public class HelperServiceDTO
    {
        public int UserId { get; set; }

        public bool Status { get; set; }

        public string Qualification { get; set; }

        public int ExperienceYears { get; set; }

        public int MinAge { get; set; }

        public int MaxAge { get; set; }

        public List<Services> Service { get; set; }
    }

    public class Services
    {
        public int ServiceId { get; set; }

        public int LocationType { get; set; }

        public string LocationName { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public bool Hour { get; set; }

        public decimal MinPriceHour { get; set; }

        public decimal MaxPriceHour { get; set; }

        public bool Day { get; set; }

        public decimal MinDayPrice { get; set; }

        public decimal MaxDayPrice { get; set; }

        public bool Month { get; set; }

        public decimal MinMonthPrice { get; set; }

        public decimal MaxMonthPrice { get; set; }

        public List<Scopes> Scopes { get; set; }
    }

    public class Scopes
    {
        public int ScopeId { get; set; }
    }
}
