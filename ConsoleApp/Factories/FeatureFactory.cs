using DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Factories
{
    static class FeatureFactory
    {
        public enum VectorLabels
        {
            Acousticness = 0,
            Dancability = 1,
            Energy = 2,
            Instrumentalness = 3,
            Key = 4,
            Liveness = 5,
            Loudness = 6,
            Speechiness = 7,
            Tempo = 8,
            Valence = 9
        }

        public static double[] BuildVector(TrackFeatures t)
        {
            double[] vector = new double[] {
                t.Acousticness,
                t.Danceability,
                t.Energy,
                t.Instrumentalness,
                t.Key,
                t.Liveness,
                t.Loudness,
                t.Speechiness,
                t.Tempo,
                t.Valence
            };

            return vector;
        }
    }
}
