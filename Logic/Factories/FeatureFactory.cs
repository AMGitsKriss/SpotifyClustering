namespace DTO.Factories
{
    public static class FeatureFactory
    {
        // TODO - This is very spotify specific, Probably belongs with helpers or the DTOs
        public static string[] BuildVector(TrackFeatures t)
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
