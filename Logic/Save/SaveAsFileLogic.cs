using DTO;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Logic.Save
{
    public class SaveAsFileLogic : ISavePlaylistLogic
    {
        private ISpotifyRepository _repo;
        private string _username;

        public SaveAsFileLogic(ISpotifyRepository repo)
        {
            _repo = repo;
        }
        public void SetUser(LoginSession user)
        {
            _repo.SetAuthorization("Bearer " + user.Tokens.AccessToken);
            _username = user.User.Username;
        }

        public void Save(IList<TrackFeatures> features, string playlistName)
        {
            var thing = features.Select(f => BuildCSVRow(f));
            if (thing.Any())
                WriteCSV(thing, $@"C:\Users\Kriss\Desktop\SPOTIFY\API DbScan Playlist {_username} {playlistName}.csv");
        }

        private void WriteCSV(IEnumerable<string[]> items, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                foreach (var item in items)
                {
                    writer.WriteLine(string.Join(", ", item));
                }
            }
        }

        public Playlist AddNewPlaylist(string username, string playlistName)
        {
            throw new NotImplementedException();
        }

        public bool AddTrack(string playlistID, List<string> uris)
        {
            throw new NotImplementedException();
        }

        private string[] BuildCSVRow(TrackFeatures t)
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
