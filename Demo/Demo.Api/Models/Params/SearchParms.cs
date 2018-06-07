﻿namespace Helpa.Api.Models.Params
{
    public class SearchParms
    {
        public int? Radius { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public int? ServiceId { get; set; }

        public int? LocationTypeId { get; set; }

        public int? ScopeId { get; set; }
    }
}