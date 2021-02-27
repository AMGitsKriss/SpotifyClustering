using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class User
    {
        public string ID { get; set; }
        [JsonProperty(PropertyName = "display_name")]
        public string Username { get; set; }
        public string Country { get; set; }
    }
}
