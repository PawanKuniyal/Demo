namespace Helpa.Api.Models.ReturnModels
{
    using Newtonsoft.Json;

    public class CreatedId
    {
        public int Id;
    }

    public class Errors
    {
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}