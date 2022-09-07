using OBSWebsocketDotNet.Types;

namespace GeoChatter.Integrations.Classes
{
    /// <summary>
    /// OBS scene model
    /// </summary>
    public class GCOBSScene : OBSScene
    {
        /// <summary>
        /// Name of scene
        /// </summary>
        public string DisplayName => Name;
    }
}
