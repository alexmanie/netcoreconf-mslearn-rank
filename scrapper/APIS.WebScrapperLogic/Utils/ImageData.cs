using System;

namespace APIS.WebScrapperLogic.Utils
{
    public class ImageData
    {
        public string URI { get; set; }
        public string Filename { get; set; }
        public int Filesize { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] BinaryData { get; set; }
        public string MD5 { get; set; }
        public string MIMEType { get; set; }
        public DateTime? StartProcessingOn { get; set; }
        public DateTime? EndProcessingOn { get; set; }
        public string Error { get; set; }
        public byte[] ThumbnailBinaryData { get; set; }
    }
}
