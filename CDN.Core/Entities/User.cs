using Newtonsoft.Json;

namespace CDN.Core.Entities
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("skillsets")]
        public string Skillsets { get; set; }
        
        [JsonProperty("hobbies")]
        public string Hobbies { get; set; }
    }
}