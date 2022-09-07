using GeoChatter.Extensions;
using GeoChatter.Helpers;
using GeoChatter.Core.Helpers;
using GeoChatter.Core.Model;
using GeoChatter.Model.Enums;
using GeoChatter.Core.Storage;
using GeoChatter.Web;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using GeoChatter.Model;
using GeoChatter.Forms;
using System.Diagnostics;

namespace GeoChatter.Test
{
    public class JSEventTests
    {
        public static Round TestRound => ClientDbCache.RunningGame.Rounds.First();
        private MainForm form;

        [SetUp]
        public void Setup()
        {
            if (form == null)
            {
                form = new MainForm(FileVersionInfo.GetVersionInfo(typeof(MainForm).Assembly.Location).FileVersion);
            }
            GameFoundStatus status = GameFoundStatus.NOTFOUND;
            GeoGuessrGame g = GeoGuessrClient.GetGameData(new() { ID = GeoGuessrClient.GetGameId(WebTests.GameAddresses["default_start"]), IsChallenge = false });
            ClientDbCache.RunningGame = g?.CreateFromGeoGuessrGame(ClientDbCache.Instance, new(), "TEST", out status);
        }

        [Test]
        public void RegisterClickEvent()
        {
            Assert.DoesNotThrow(() => JsToCsHelper.GetRegisterClickToJsScript());
        }

        [Test]
        public void StartGameEvent()
        {
            Assert.DoesNotThrow(() => JsToCsHelper.GetStartGameJsScript(ClientDbCache.RunningGame));
        }

        [Test]
        public void StartRoundJsScript()
        {
            Assert.DoesNotThrow(() => JsToCsHelper.GetStartRoundJsScript(TestRound));
        }

        [Test]
        public void EndRoundJsScript()
        {
            Assert.DoesNotThrow(() => JsToCsHelper.GetEndRoundJsScript(TestRound));
        }

        [Test]
        public void SendGuessObjToJsScript()
        {
            bool validCoordinates = GCUtils.ValidateAndFixCoordinates("40", "-80", out double lat, out double lng);
            Player player = new() { PlayerName = "rhinoooo_", PlayerFlag = "TR", CountryStreak = 0, Guesses = new List<Guess>() };

            Guess guess = new()
            {
                GuessLocation = new Coordinates(lat, lng),
                Player = player
            };

            string code = BorderHelper.GetFeatureHitBy(new double[] { lng, lat }, out GeoJson geo, out Feature hitFeature, out Polygon hitPoly);

            Country country = CountryHelper.GetCountryInformation(code, geo, hitFeature, true, out Country exactcountry);

            Assert.IsNotNull(country);
            Assert.IsNotNull(exactcountry);
            Assert.IsFalse(string.IsNullOrWhiteSpace(country.Code));
            Assert.IsFalse(string.IsNullOrWhiteSpace(country.Name));
            Assert.IsFalse(string.IsNullOrWhiteSpace(exactcountry.Code));
            Assert.IsFalse(string.IsNullOrWhiteSpace(exactcountry.Name));

            guess.Country = country;
            guess.CountryExact = exactcountry;

            Assert.DoesNotThrow(() => TestRound.AddGuess(guess, ScoreFormulatorExpression.Create, ScoreFormulatorVariable.GetAsDouble));

            Assert.DoesNotThrow(() => JsToCsHelper.GetSendGuessObjToJsScript(guess));
        }

        [Test]
        public void EndGameJsScript()
        {
            Assert.DoesNotThrow(() => JsToCsHelper.GetEndGameJsScript(ClientDbCache.RunningGame));
        }

        [Test]
        public void ExitGameJsScript()
        {
            Assert.AreEqual(string.Empty, JsToCsHelper.GetExitGameJsScript(null));
            Assert.DoesNotThrow(() => JsToCsHelper.GetExitGameJsScript(ClientDbCache.RunningGame));
        }

        [Test]
        public void RefreshGameJsScript()
        {
            Assert.AreEqual(string.Empty, JsToCsHelper.GetRefreshGameJsScript(null));
            Assert.DoesNotThrow(() => JsToCsHelper.GetRefreshGameJsScript(ClientDbCache.RunningGame));
        }
    }
}