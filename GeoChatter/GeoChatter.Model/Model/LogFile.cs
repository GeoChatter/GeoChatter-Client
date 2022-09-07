using System;

namespace GeoChatter.Model
{
    public class LogFile
    {
        public int Id { get; set; }
        public string Channel { get; set; }
        public string FileName { get; set; }
        public DateTime UploadDate { get; set; }
        public byte[] Data { get; set; }
    }
}
