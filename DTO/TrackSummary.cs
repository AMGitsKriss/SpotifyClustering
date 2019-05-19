using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class TrackSummary
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
    }

    public class TrackContainer
    {
        public TrackSummary Track { get; set; }
    }
}
