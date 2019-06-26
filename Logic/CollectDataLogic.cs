using DTO;
using LogicContracts;
using RepositoryContracts;
using Repsoitory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
    public class CollectDataLogic : ICollectDataLogic
    {
        private ISpotifyRepository _repo;

        public CollectDataLogic(string apiKey)
        {
            _repo = new SpotifyRepository(apiKey);
        }

        public List<Playlist> GetPlaylists(string username)
        {
            return _repo.GetPlaylists(username);
        }

        /// <summary>
        /// Iterate over a list of playlist IDs to fetch a list of tracks contained therein.
        /// </summary>
        /// <param name="playlistIDs"></param>
        /// <returns>A collection of tracks with duplicates removed</returns>
        public List<TrackSummary> GetPlaylistTracks(List<string> playlistIDs)
        {
            List<TrackSummary> trackSet = new List<TrackSummary>();

            foreach (string id in playlistIDs)
            {
                List<TrackSummary> playlistTracks = _repo.GetTrackList(id);
                trackSet.AddRange(playlistTracks);
            }

            return trackSet;
        }

        public List<TrackFeatures> GetTrackFeatures(List<TrackSummary> trackList)
        {
            List<string> trackIDs = trackList.Select(x => x.ID).ToList();
            List<TrackFeatures> result = _repo.GetTrackFeatures(trackIDs);

            foreach (var item in result)
            {
                TrackSummary itemSummary = trackList.First(x => x.ID.Equals(item.ID));
                item.Name = itemSummary.Name;
                item.Artist = itemSummary.Artist;
            }

            return result;

        }
    }
}
