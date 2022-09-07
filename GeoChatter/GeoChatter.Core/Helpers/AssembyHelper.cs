using System;
using System.IO;
using System.Reflection;

namespace GeoChatter.Core.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssembyHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().Location;
                UriBuilder uri = new(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
