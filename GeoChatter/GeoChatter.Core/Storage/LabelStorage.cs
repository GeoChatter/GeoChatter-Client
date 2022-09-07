using GeoChatter.Core.Common.Extensions;
using GeoChatter.Model.Enums;
using log4net;
using System.IO;
using System.Windows.Forms;

namespace GeoChatter.Core.Storage
{
    /// <summary>
    /// Label storage methods
    /// </summary>
    public static class LabelStorage
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(LabelStorage));

        /// <summary>
        /// Write label of <paramref name="type"/> to <paramref name="path"/> with <paramref name="value"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="path"></param>
        public static void WriteLabel(LabelType type, string value, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Application.StartupPath + "\\labels";
            }

            DirectoryInfo info = new(path);
            if (!info.Exists)
            {
                info.Create();
            }

            path = path + "\\" + type.ToString() + ".txt";
            try
            {
                File.WriteAllText(path, value);
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }
    }


}
