using DTO;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using LogicContracts;
using RepositoryContracts;

namespace Logic.Save
{
    public class SpotifySaveLogic : ISavePlaylistLogic
    {
        private ISpotifyRepository _repo;
        private string _username;

        public SpotifySaveLogic(ISpotifyRepository repo)
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

            Playlist newPlaylist = AddNewPlaylist(_username, playlistName);
            List<string> trackList = features.Select(f => f.URI).ToList();

            int batchSize = 100;
            int done = 0;
            while (done < trackList.Count)
            {
                List<string> trackBatch = trackList.GetRange(done, Math.Min(batchSize, trackList.Count - done));
                AddTrack(newPlaylist.ID, trackBatch);
                done += batchSize;
            }
        }

        public Playlist AddNewPlaylist(string username, string playlistName)
        {
            BasePlaylist request = new BasePlaylist()
            {
                Name = playlistName,
                Public = false
            };
            return _repo.AddNewPlaylist(username, request);
        }

        public bool AddTrack(string playlistID, List<string> uris)
        {
            AddTrackRequest request = new AddTrackRequest()
            {
                uris = uris
            };
            return _repo.AddTrack(playlistID, request);
        }
    }
}
