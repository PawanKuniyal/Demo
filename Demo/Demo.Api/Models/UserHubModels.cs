namespace Helpa.Api.Models
{
    using System.Collections.Generic;

    public class UserHubModels
    {
        public string UserName { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }
}