using DTO;
using LogicContracts;
using RepositoryContracts;
using Repsoitory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
    public class PostDataLogic : IPostDataLogic
    {
        private ISpotifyRepository _repo;

        public PostDataLogic(string apiKey)
        {
            _repo = new SpotifyRepository(apiKey);
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
