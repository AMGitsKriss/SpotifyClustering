using DTO;
using Logic;
using LogicContracts;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicTests
{
    [TestFixture, Category("Logic")]
    public class CollectDataLogicTest
    {
        [Test]
        public void GetPlaylists()
        {
            string token = "Bearer BQB8KITNnaNPyn2sXWzHF5o7zWm3J0Q1U1YuVO9SnRCdwLRG-9A6Jv7PLBnnychs6etQmPYpdjc2eVKmpxpUva-jTe6ljBYzGuEQVhyzWTgUfg_hBdqKGXXy20VGayX-YX8muomXcxtb7wkoSYk";
            ICollectDataLogic logic = new CollectDataLogic(token);
            List<Playlist> result = logic.GetPlaylists("AMGitsKriss");
            Assert.NotNull(result);
        }

        [Test]
        public void GetTracks()
        {
            string token = "Bearer BQB8KITNnaNPyn2sXWzHF5o7zWm3J0Q1U1YuVO9SnRCdwLRG-9A6Jv7PLBnnychs6etQmPYpdjc2eVKmpxpUva-jTe6ljBYzGuEQVhyzWTgUfg_hBdqKGXXy20VGayX-YX8muomXcxtb7wkoSYk";
            ICollectDataLogic logic = new CollectDataLogic(token);
            List<string> playlists = new List<string>() { "5MFWhQq4CMXNFRhzvyjqQn", "0mE2MHJvMWbrXhq6JdCWez", "7hurzbOfJPLydJRDku2dy2" };
            HashSet<TrackSummary> result = logic.GetPlaylistTracks(playlists);
            Assert.NotNull(result);
        }

        [Test]
        public void GetTrackFeatures()
        {
            string token = "Bearer BQB8KITNnaNPyn2sXWzHF5o7zWm3J0Q1U1YuVO9SnRCdwLRG-9A6Jv7PLBnnychs6etQmPYpdjc2eVKmpxpUva-jTe6ljBYzGuEQVhyzWTgUfg_hBdqKGXXy20VGayX-YX8muomXcxtb7wkoSYk";
            ICollectDataLogic logic = new CollectDataLogic(token);
            List<string> trackIDs = new List<string>() { "7bX77sZFzgQlBGVIhNVDK5", "0xM88xobymkMgg46MStfnV", "5p9XWUdvbUzmPCukOmwoU3" };
            List<TrackFeatures> result = logic.GetTrackFeatures(null);
            Assert.NotNull(result);
        }

    }
}
