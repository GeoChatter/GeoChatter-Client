using System.Runtime.Serialization;
namespace GeoChatter.Model
{
    public class ApiClient
    {
        [DataMember(Name = "channelname")]
        public string ChannelName { get; set; }
        [DataMember(Name = "botname")]
        public string BotName { get; set; }
        [DataMember(Name = "version")]
        public string Version { get; set; }

        public override string ToString()
        {
            return $"{ChannelName} ({BotName}) ({Version})";
        }

    }
}
