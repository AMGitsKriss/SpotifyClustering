using DTO;
using DTO.Factories;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Logic.Save
{
    // TODO - Pull all the save functionality out of PlaylistManager. This belongs with the spotify logic
    public class SaveAsFile : ISaveStrategy
    {
        private ISpotifyRepository _repo;
        private string _username;

        public SaveAsFile(ISpotifyRepository repo)
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
            var thing = features.Select(f => FeatureFactory.BuildVector(f));
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

        private string Escape(string val)
        {
            if (val == null) return null;
            return val.Replace("\"", "\"\"");
        }

        public Playlist AddNewPlaylist(string username, string playlistName)
        {
            throw new NotImplementedException();
        }

        public bool AddTrack(string playlistID, List<string> uris)
        {
            throw new NotImplementedException();
        }
    }
}
