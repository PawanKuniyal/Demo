namespace Helpa.Entities.CustomEntities
{
    using System.Collections.Generic;

    public class TrustDTO
    {
        public int HelperId { get; set; }

        public string SelfIntroduction { get; set; }

        public List<Certificates> Certificate { get; set; }

        public List<Carousels> Carousels { get; set; }
    }

    public class Certificates
    {
        public string Certificate { get; set; }
    }

    public class Carousels
    {
        public string Images { get; set; }
    }
}
