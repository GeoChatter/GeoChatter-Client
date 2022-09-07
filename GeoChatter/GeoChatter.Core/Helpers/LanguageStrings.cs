using Antlr4.StringTemplate;
using GeoChatter.Core.Model;
using GeoChatter.Core.Storage;
using GeoChatter.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace GeoChatter.Core.Helpers
{
    /// <summary>
    /// Strings for messages and other resources
    /// </summary>
    public static class LanguageStrings
    {

        /// <summary>
        /// Chat message strings collection
        /// </summary>
        public static List<ChatMessage> Strings { get; set; } = new();
        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<string, string> ValuePairs { get; } = new();

        /// <summary>
        /// Initialize collections from <paramref name="appSettings"/>
        /// </summary>
        /// <param name="appSettings"></param>
        public static void Initialize(ApplicationSettingsBase appSettings)
        {
            if (appSettings == null)
            {
                return;
            }

            using ClientDbContext db = new();
            if (Strings.Count == 0)
            {
                Strings = db.ChatMessages.ToList();
            }

            if (!ValuePairs.ContainsKey("channelName"))
            {
                ValuePairs.Add("channelName", appSettings["ChannelId"]?.ToString());
            }
            else
            {
                ValuePairs["channelName"] = appSettings["ChannelId"]?.ToString();
            }

        }

        public static void SetMapIdentifier(string mapId)
        {

            if (!ValuePairs.ContainsKey("botName"))
            {
                ValuePairs.Add("botName", mapId);
            }
            else
            {
                ValuePairs["botName"] = mapId;
            }
        }

        /// <summary>
        /// Save current strings to DB
        /// </summary>
        public static void Save()
        {
            using ClientDbContext db = new();
            foreach (ChatMessage message in Strings.Where(c => c.Modified))
            {
                ChatMessage dbMsg = db.ChatMessages.FirstOrDefault(c => c.Id == message.Id);
                if (dbMsg != null)
                {
                    dbMsg.Message = message.Message;
                }
            }
            db.ChangeTracker.AcceptAllChanges();
            db.SaveChanges();
        }
        // public static void Save() { storage.Save(Strings); }


        /// <summary>
        /// Get string resource named <paramref name="v"/> 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="customValues"></param>
        /// <returns></returns>
        public static string Get(string v, Dictionary<string, string> customValues = null)
        {
            ChatMessage msg = Strings.FirstOrDefault(c => c.Name == v);
            if (msg != null)
            {
                Template stringTemplate = new(msg.Message);
                if (customValues != null)
                {
                    foreach (KeyValuePair<string, string> kvp in customValues)
                    {
                        stringTemplate.Add(kvp.Key, kvp.Value);
                    }
                }

                foreach (KeyValuePair<string, string> kvp in ValuePairs)
                {
                    stringTemplate.Add(kvp.Key, kvp.Value);
                }

                stringTemplate.Add("currentTime", DateTime.Now.ToLongTimeString());



                return stringTemplate.Render(CultureInfo.InvariantCulture);
            }

            return string.Empty;
        }
    }


}
