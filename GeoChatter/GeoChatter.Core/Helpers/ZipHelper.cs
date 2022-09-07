using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Helpers;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using static System.Net.WebRequestMethods;

namespace GeoChatter.Core.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class ZipHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputName"></param>
        /// <returns></returns>
        public static byte[] ZipLogFiles(string outputName)
        {
            string[] files = Directory.GetFiles(AssembyHelper.AssemblyDirectory, "*.log", SearchOption.TopDirectoryOnly);
            using (ZipOutputStream OutputStream = new ZipOutputStream(System.IO.File.Create(outputName)))
            {
                // Define the compression level
                // 0 - store only to 9 - means best compression
                OutputStream.SetLevel(9);

                byte[] buffer = new byte[4096];

                files.ForEach(f =>
                {


                  

                    // Using GetFileName makes the result compatible with XP
                    // as the resulting path is not absolute.
                    ZipEntry entry = new ZipEntry(Path.GetFileName(f));

                    // Setup the entry data as required.

                    // Crc and size are handled by the library for seakable streams
                    // so no need to do them here.

                    // Could also use the last write time or similar for the file.
                    entry.DateTime = DateTime.Now;
                    OutputStream.PutNextEntry(entry);

                    using (FileStream fs = System.IO.File.Open(f, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {

                        // Using a fixed size buffer here makes no noticeable difference for output
                        // but keeps a lid on memory usage.
                        int sourceBytes;

                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            OutputStream.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                 



                });

                // Finish/Close arent needed strictly as the using statement does this automatically

                // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                // the created file would be invalid.
                OutputStream.Finish();

                // Close is important to wrap things up and unlock the file.
                OutputStream.Close();
            }
            byte[] logContent;
            using (FileStream stream = System.IO.File.Open(outputName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                logContent = new byte[stream.Length];
                stream.Read(logContent, 0, logContent.Length);
            }

            files.ForEach(f =>
            {
                try{ System.IO.File.Delete(f); }catch (IOException) { }
            });
            return logContent;
        }
    }
}
