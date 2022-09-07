using GeoChatter.Core.Helpers;
using NUnit.Framework;

namespace GeoChatter.Test
{
    public class JSExtensionTests
    {
        [Test]
        public void LoadExtensionFromRawScriptAndUpdate()
        {
            string testname = "Wiki Summary";
            string testversion = "0.1.0";
            string testdescription = "Display Wikipedia summary of the Geoguessr locations. Works with streaks, single player 5 round games and challenges.";
            string testsource = "https://github.com/semihM/GeoGuessrScripts/blob/main/WikiSummary";
            string testupdate = "https://greasyfork.org/en/scripts/436842-wiki-summary/code/user.js";
            string testblock = @$"
// ==UserScript==
// @name         {testname}
// @include      /^(https?)?(\:)?(\/\/)?([^\/]*\.)?geoguessr\.com($|\/.*)/
// @version      {testversion}
// @description  {testdescription}
// @author       semihM (aka rhinoooo_), MiniKochi
// @source       {testsource}
// @supportURL   https://github.com/semihM/GeoGuessrScripts/issues
// @updateURL    {testupdate}
// @require      http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js
// @require      http://code.jquery.com/jquery-3.4.1.min.js
// ==/UserScript==
";
            JSUserScript ext = new(testname, testblock, JSWrapperType.Raw);
            Assert.IsTrue(ext.PrepareForExecution());

            ext.DiscoverAndSetAll();

            Assert.AreEqual(testname, ext.Name, $"Extension name was wrong");
            Assert.AreEqual(testversion, ext.Version, $"Extension version was wrong");
            Assert.AreEqual(testdescription, ext.Description, $"Extension description was wrong");
            Assert.AreEqual(testsource, ext.Source, $"Extension source was wrong");
            Assert.AreEqual(testupdate, ext.AutoUpdateURL, $"Extension auto update was wrong");

            Assert.IsTrue(ext.CheckForUpdates(ext.AutoUpdateURL), "Auto update failed");

            Assert.IsFalse(string.IsNullOrWhiteSpace(ext.Version));
            Assert.AreNotEqual(ext.Version, testversion, "Version didn't change after update");
        }

        [Test]
        public void CheckGreasyForkSourceLinkMatches()
        {
            string full = "https://greasyfork.org/en/scripts/436842-wiki-summary/code/user.js";
            string[] sources = new string[]
            {
                "https://greasyfork.org/en/scripts/436842-wiki-summary",
                "https://greasyfork.org/en/scripts/436842-wiki-summary/code",
                full,
            };

            foreach (string source in sources)
            {
                Assert.AreEqual(full, JSUserScript.GetFixedSourceURL(source), $"Failed to match greasyfork for {source}");
            }
        }

        [Test]
        public void CheckOpenUserJSSourceLinkMatches()
        {
            string full = "https://openuserjs.org/src/scripts/drparse/GeoNoCar.user.js";
            string[] sources = new string[]
            {
                "https://openuserjs.org/scripts/drparse/GeoNoCar",
                "https://openuserjs.org/scripts/drparse/GeoNoCar/source",
                full,
            };

            foreach (string source in sources)
            {
                Assert.AreEqual(full, JSUserScript.GetFixedSourceURL(source), $"Failed to match openuserjs for {source}");
            }
        }
    }
}