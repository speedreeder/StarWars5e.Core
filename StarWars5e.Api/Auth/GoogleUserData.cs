using Newtonsoft.Json;

namespace StarWars5e.Api.Auth
{
    public class GoogleUserData
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Id { get; set; }

        [JsonProperty("given_name")]
        public string FirstName { get; set; }

        [JsonProperty("family_name")]
        public string LastName { get; set; }
    }
}
