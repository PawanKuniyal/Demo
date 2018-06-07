namespace Helpa.Entities.CustomEntities
{
    using System;

    public class ReviewAndRatingDTO
    {
        public int UserId { get; set; }

        public int HelperId { get; set; }

        public Ratings? Ratings { get; set; }

        public string Reviews { get; set; }
    }

    [Flags]
    public enum Ratings
    {
        Poor = 1,
        Fair = 2,
        Good = 3,
        VeryGood = 4,
        Excellent = 5
    }
}
