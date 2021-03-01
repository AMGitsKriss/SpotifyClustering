using DTO;

namespace Website.Builders
{
    public static class FeatureBuilder
    {
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

        public static string[] BuildCSVRow(TrackFeatures t)
        {
            string[] vector = new string[] {
                $"{t.Name} - {t.Artist}",
                t.Acousticness.ToString(),
                t.Danceability.ToString(),
                t.Energy.ToString(),
                t.Instrumentalness.ToString(),
                (((double)t.Key+1.0) / 12.0).ToString(),
                t.Liveness.ToString(),
                ((t.Loudness+60) / 60).ToString(),
                t.Speechiness.ToString(),
                (t.Tempo / 200).ToString(),
                t.Valence.ToString()
            };

            return vector;
        }
    }
}
