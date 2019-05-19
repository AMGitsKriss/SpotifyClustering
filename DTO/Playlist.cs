using Newtonsoft.Json;
using System;

namespace DTO
{
    public class Playlist
    {
        public string ID { get; set; }
        public string Name { get; set; }
        [JsonProperty("total")]
        public int Count { get; set; }
        public string URI { get; set; }
    }
}
