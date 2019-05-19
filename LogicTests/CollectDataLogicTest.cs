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
            string token = "Bearer BQDEDHHGCjEeTLN_Ir1WePpytroZjSWE3IvaFbDDRffoXXGdqth1OEhaY-B_ht-UAmeI_PF_nV7TcrW02pQBoRez4SpsYholOPo_N-ZT2sGjrGB39cg_qBz2Lbzo1z6VKBBGFVVjrLmdHUaGP8E";
            ICollectDataLogic logic = new CollectDataLogic(token);
            List<Playlist> result = logic.GetPlaylists("AMGitsKriss");
            Assert.NotNull(result);
        }

        [Test]
        public void GetTracks()
        {
            string token = "Bearer BQDEDHHGCjEeTLN_Ir1WePpytroZjSWE3IvaFbDDRffoXXGdqth1OEhaY-B_ht-UAmeI_PF_nV7TcrW02pQBoRez4SpsYholOPo_N-ZT2sGjrGB39cg_qBz2Lbzo1z6VKBBGFVVjrLmdHUaGP8E";
            ICollectDataLogic logic = new CollectDataLogic(token);
            List<string> playlists = new List<string>() { "5MFWhQq4CMXNFRhzvyjqQn", "0mE2MHJvMWbrXhq6JdCWez", "7hurzbOfJPLydJRDku2dy2" };
            HashSet<TrackSummary> result = logic.GetPlaylistTracks(playlists);
            Assert.NotNull(result);
        }

    }
}
