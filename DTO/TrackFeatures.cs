using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class TrackFeatures : TrackSummary
    {
        public float Danceability { get; set; }
        public float Energy { get; set; }
        public int Key { get; set; }
        public float Loudness { get; set; }
        public int Mode { get; set; } // 1 = Major, = 0 Minor
        public float Speechiness { get; set; }
        public float Acousticness { get; set; }
        public float Instrumentalness { get; set; }
        public float Liveness { get; set; }
        public float Valence { get; set; }
        public float Tempo { get; set; }
        public string URI { get; set; }
        public int Duration { get; set; }
        public int Time_Signiture { get; set; }
    }
}
