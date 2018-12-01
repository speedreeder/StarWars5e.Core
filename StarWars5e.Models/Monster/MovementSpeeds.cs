using Newtonsoft.Json;

namespace StarWars5e.Models.Monster
{
    public class MovementSpeeds
    {
        [JsonProperty("walkingSpeed")]
        public int Walking { get; set; }

        [JsonProperty("climbingSpeed")]
        public int Climbing { get; set; } // climb

        [JsonProperty("swimmingSpeed")]
        public int Swimming { get; set; } // swim

        [JsonProperty("flyingSpeed")]
        public int Flying { get; set; } // fly
    }
}