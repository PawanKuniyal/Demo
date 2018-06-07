using System.Collections.Generic;

namespace Helpa.Entities.CustomEntities
{
    public class HelpersHomeDTO
    {
        public string LocalityName { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public int NumberOfHelpersInLocality { get; set; }

        public ICollection<HelpersInLocalty> HelpersInLocalties { get; set; }
    }

    public class HelpersInLocalty
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string ProfilePicture { get; set; }

        public string AverageRating { get; set; }

        public string RatingCount { get; set; }

        public string Service { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
