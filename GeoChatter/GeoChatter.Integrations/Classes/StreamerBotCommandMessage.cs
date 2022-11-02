using GeoChatter.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoChatter.Integrations.StreamerBot
{
    public class StreamerBotCommandMessage
    {
        
    public string timeStamp { get; set; }
    public StreamerBotEvent Event { get; set; }
    public StreamerBotCommandMessagePart data { get; set; }

}
    public class StreamerBotEvent
    {
        public string source { get; set; }
        public string type { get; set; }
    }
    public class StreamerBotCommandMessagePart
    {
        public string command { get; set; }
        public int counter { get; set; }
        public int userCounter { get; set; }
        public string message { get; set; }
        public string completeMessage { get => command + " " + message; }
        public StreamerBotUser user { get; set; }


    }
    public class StreamerBotUser
    {
        public string id { get; set; }
        public string login { get; set; }

        public string name { get; set; }
        public string username { get; set; }
        public string display_name { get; set; }
        public bool subscribed { get; set; }
        public int role { get; set; }

        public string type { get; set; }
    }

}
