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
            apiToken = "BQBEKUkBzXTuKfmbmI-ksL1AmI55pV_vwUc53m9kq6__iTEKZmsjWVr3KqAcu9MNvNVlgqnwClQy5Pop0vysh8pyH9wJJAExtz7bTn1vvsgv5oSMRUbQkM5MZrTxshyRrDL6HpD2wVJJyZpnCMrdH6YAZiW1qLvzPjWGceWYkdIQWiOfAC9j2ev3UQfKNzFpJYuxGu96YxAUZd0fBweqStKAdMq6W0UXAvxyn9gVAuCDP-gRiurshKlmkIq5ZopWbcwjD6i00b1g1vf-nA";
        }

        [Test]
        public void Test()
        {
            PLM manager = new PLM(apiToken, "amgitskriss", new SaveAsFile());
            manager.Organise();
        }
    }
}