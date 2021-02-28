using DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlaylistManager.Factories
{
    static class FeatureFactory
    {
        // TODO - This is very spotify specific, Probably belongs with helpers or the DTOs
        public static double[] BuildVector(TrackFeatures t)
        {
            double[] vector = new double[] {
                t.Acousticness,
                t.Danceability,
                t.Energy,
                t.Instrumentalness,
                ((double)t.Key+1.0) / 12.0,
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
