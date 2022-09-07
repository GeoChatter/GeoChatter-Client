using CefSharp;

namespace GeoChatter.Core.Handlers
{
    /// <summary>
    /// <see cref="GCSchemeHandler"/> factory
    /// </summary>
    public class GCSchemeHandlerFactory : ISchemeHandlerFactory
    {
        /// <summary>
        /// Name to use for the scheme handler
        /// </summary>
        public const string SchemeName = "schemehandler";

        /// <summary>
        /// GeoChatter version
        /// </summary>
        public static string Version { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        public GCSchemeHandlerFactory(string version)
        {
            Version = version;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="schemeName"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return new GCSchemeHandler();
        }
    }
}
