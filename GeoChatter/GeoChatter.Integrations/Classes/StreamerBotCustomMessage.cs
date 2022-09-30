using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoChatter.Integrations.Classes
{
    

    public class CustomData
    {
        public string Data { get; set; }
    }

  public class CustomEvent
    {
        public string Source { get; set; }
        public string Type { get; set; }
    }

    public class StreamerBotCustomMessage
    {
        public DateTime timeStamp { get; set; }
        public CustomEvent Event { get; set; }
        public CustomData Data { get; set; }
    }

}
