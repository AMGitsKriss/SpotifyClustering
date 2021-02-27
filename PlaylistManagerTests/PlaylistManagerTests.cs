using NUnit.Framework;
using PlaylistManager.Strategies.Save;
using PLM = PlaylistManager.PlaylistManager;

namespace PlaylistManagerTests
{
    public class PlaylistManagerTests
    {
        string apiToken;

        [SetUp]
        public void Setup()
        {
            apiToken = "BQB1TER5BBLAm0V8kceJ_5KUQKZCdqgMLBWxwgmSoQrHftH6tcUx8DYagnX-GkdzR7CB5MWQ7NeB0v6Vz9FR5epsUee7qmKLlv1_hKwjM_SMnlQPONe43LUz8Mg-gsSJS3FxePxMWaC0U3QfU6byl_JlWbf6z0LfIEUN9XzjWL-5lhxCpXF4IONZsGGg6pYB_tc1WChuGicWzFBv9YTdFuk2fRJf-CsuAknjgBIWR2GqgupzH4OgBX_Jk64h-UORd11BfpptUK7HFmInAIvA";
        }

        [Test]
        public void Test()
        {
            PLM manager = new PLM(apiToken, "amgitskriss", new PushPlaylist(new Logic.SpotifyLogic("Bearer " + apiToken)));
            manager.Organise();
        }
    }
}