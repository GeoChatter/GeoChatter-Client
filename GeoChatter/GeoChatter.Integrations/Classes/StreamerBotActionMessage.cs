using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoChatter.Integrations.Classes
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Arguments
    {
        public string actionId { get; set; }
        public string user { get; set; }
        public string userName { get; set; }
        public string userId { get; set; }
        public string userType { get; set; }
        public bool isSubscribed { get; set; }
        public bool isModerator { get; set; }
        public bool isVip { get; set; }
        public string eventSource { get; set; }
        public string broadcastUser { get; set; }
        public string broadcastUserName { get; set; }
        public int broadcastUserId { get; set; }
        public bool broadcasterIsAffiliate { get; set; }
        public bool broadcasterIsPartner { get; set; }
        public string runningActionId { get; set; }
    }

    public class Data
    {
        public string id { get; set; }
        public string name { get; set; }
        public Arguments arguments { get; set; }
        public User user { get; set; }
    }

    public class Event
    {
        public string source { get; set; }
        public string type { get; set; }
    }

    public class StreamerBotActionMessage
    {
        public DateTime timeStamp { get; set; }
        public Event @event { get; set; }
        public Data data { get; set; }
    }

    public class User
    {
        public string display { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int role { get; set; }
        public bool subscribed { get; set; }
        public string type { get; set; }
    }


}
