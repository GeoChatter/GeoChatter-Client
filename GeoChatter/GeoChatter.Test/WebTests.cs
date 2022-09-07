using GeoChatter.Core.Helpers;
using GeoChatter.Core.Model;
using GeoChatter.Model;
using GeoChatter.Web;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoChatter.Test
{
    public class WebTests
    {
        public static Dictionary<string, string> GameAddresses { get; } = new()
        {
            { "streaks_states_start", "https://www.geoguessr.com/game/KErfEr1fSrHcXDfd" },
            { "streaks_states_ended", "https://www.geoguessr.com/game/bK5qmSVerlbwh4CU" },

            { "streaks_country_start", "https://www.geoguessr.com/game/6ipwSjHSSW8rGX1y" },
            { "streaks_country_ended", "https://www.geoguessr.com/game/6tBrzep3g45HZMQr" },

            { "default_start", "https://www.geoguessr.com/game/fx3EvXwFxjpqQ3K5" },
            { "default_ended", "https://www.geoguessr.com/game/WCGAz8nLltzp1gWD" },
        };

        [SetUp]
        public void Setup()
        {
            // TODO: Remove or retrieve from server
            TwitchHelper.ClientSecret = "q31gxrkj1ina5c1jvs59m5vxufumml";
        }

        [Test]
        public void RequestGeoGuessrGameDataSuccess()
        {
            foreach (KeyValuePair<string, string> pair in GameAddresses)
            {
                string a = pair.Value;
                GameID gameid = new() { ID = GeoGuessrClient.GetGameId(a), IsChallenge = GeoGuessrClient.IsChallengeGame(a) };
                GeoGuessrGame geoGame = GeoGuessrClient.GetGameData(gameid);
                Assert.IsNotNull(geoGame, "GeoGuessr game data request failed for " + pair.Key);
            }
        }


        [Test]
        public void RequestTwitchDataByNameFromTwitchHelperSuccess()
        {
            Task<Player>? task = TwitchHelper.GetUserDataFromTwitch(string.Empty, "rhinoooo_");
            task.Wait();

            Assert.IsTrue(task.IsCompletedSuccessfully, "Twitch API user data by name failed");

            Assert.IsNotNull(task.Result, "Twitch API user data null by name");
            Assert.AreEqual("rhinoooo_", task.Result.DisplayName);
        }

        [Test]
        public void RequestTwitchDataByIDFromTwitchHelperSuccess()
        {
            Task<Player>? task = TwitchHelper.GetUserDataFromTwitch("206992018", string.Empty);
            task.Wait();

            Assert.IsNotNull(task.Result, "Twitch API user data null by id");
            Assert.AreEqual("NoBuddyIsPerfect", task.Result.DisplayName);

            Assert.IsTrue(task.IsCompletedSuccessfully, "Twitch API user data by id failed");
        }

        [Test]
        public void RequestTwitchDataFromTwitchHelperFail()
        {
            Task<Player>? task = TwitchHelper.GetUserDataFromTwitch(string.Empty, string.Empty);
            task.Wait();

            Assert.IsTrue(task.IsCompletedSuccessfully, "Twitch API user data failed to fail!");
            Assert.IsNotNull(task.Result, "Twitch API user data null on fail");
            Assert.AreEqual(0, task.Result.Id, "Twitch API user was created an invalid instance");
        }
    }
}