using PlaylistManager.Models;
using DTO;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using LogicContracts;

namespace PlaylistManager.Strategies.Save
{
    public class PushPlaylist : ISaveStrategy
    {
        private ISpotifyLogic logic;

        public PushPlaylist(ISpotifyLogic logic)
        {
            this.logic = logic;
        }

        public void Save(IList<TrackFeatures> features, IList<Vector> groupedTracks, string username)
        {
            int clusters = groupedTracks.Max(c => c.Cluster);

            for (int i = 1; i <= clusters; i++)
            {
                Playlist newPlaylist = logic.AddNewPlaylist(username, $"API DbScan Playlist {i}");
                IEnumerable<TrackFeatures> featureList = features.Where(f => groupedTracks.Where(t => t.Cluster == i).Select(t => t.ID).Contains(f.ID));
                List<string> trackList = featureList.Select(f => f.URI).ToList();

                int batchSize = 100;
                int done = 0;
                while (done < trackList.Count)
                {
                    List<string> trackBatch = trackList.GetRange(done, Math.Min(batchSize, trackList.Count - done));
                    logic.AddTrack(newPlaylist.ID, trackBatch);
                    done += batchSize;
                }
            }
        }
    }
}
