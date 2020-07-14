using DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlaylistManager.Factories
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
                (t.Key+1) / 12,
                t.Liveness,
                (t.Loudness+60) / 60,
                t.Speechiness,
                t.Tempo / 200,
                t.Valence
            };

            return vector;
        }

        public static double[] GenerateWeights(
            double acousticness = 1,
            double danceability = 1,
            double energy = 1,
            double instrumentalness = 1,
            double key = 1,
            double liveness = 1,
            double loudness = 1,
            double speechiness = 1,
            double tempo = 1,
            double valence = 1
            )
        {
            double[] vector = new double[] {
                acousticness,
                danceability,
                energy,
                instrumentalness,
                key,
                liveness,
                loudness,
                speechiness,
                tempo,
                valence
            };
            return vector;
        }
    }
}
