using Newtonsoft.Json;

namespace Netcoreconf.Mslearn
{
    internal class User
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        [JsonProperty("statuspoints")]
        public string StatusPoints { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}