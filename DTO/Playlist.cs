using Newtonsoft.Json;
using System;

namespace DTO
{
    public class Playlist : BasePlaylist
    {
        public string ID { get; set; }
        [JsonProperty("total")]
        public int Count { get; set; }
        public string URI { get; set; }
    }

    public class BasePlaylist
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "public")]
        public bool @Public { get; set; }
    }
}
