using APIS.WebScrapperLogic.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;

namespace APIS.WebScrapperLogic.ImagesMatcher
{
    public static class BinaryImageHelper
    {
        /// <summary>
        /// Extracts MD5 from image's binary data
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static string GetMD5(byte[] image)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] rgbaHashArray = md5.ComputeHash(image);
            string rgbaHash = BitConverter.ToString(rgbaHashArray).Replace("-", "").ToLower();

            return rgbaHash;
        }

        public static string GetImageFormat(Image img)
        {
            if (img.RawFormat.Equals(ImageFormat.Jpeg))
                return ImageFormat.Jpeg.ToString();
            if (img.RawFormat.Equals(ImageFormat.Bmp))
                return ImageFormat.Bmp.ToString();
            if (img.RawFormat.Equals(ImageFormat.Png))
                return ImageFormat.Png.ToString();
            if (img.RawFormat.Equals(ImageFormat.Emf))
                return ImageFormat.Emf.ToString();
            if (img.RawFormat.Equals(ImageFormat.Exif))
                return ImageFormat.Exif.ToString();
            if (img.RawFormat.Equals(ImageFormat.Gif))
                return ImageFormat.Gif.ToString();
            if (img.RawFormat.Equals(ImageFormat.Icon))
                return ImageFormat.Icon.ToString();
            if (img.RawFormat.Equals(ImageFormat.MemoryBmp))
                return ImageFormat.MemoryBmp.ToString();
            if (img.RawFormat.Equals(ImageFormat.Tiff))
                return ImageFormat.Tiff.ToString();
            else
                return ImageFormat.Wmf.ToString();
        }

        internal static List<ImageData> GetImageFromURI(IEnumerable<string> uriList)
        {
            var result = new List<ImageData>();
            using (var webcli = new WebClient())
            {
                foreach (var uri in uriList)
                {
                    var image = new ImageData
                    {
                        StartProcessingOn = DateTime.Now
                    };

                    try
                    {
                        image.URI = uri;

                        var URI = new Uri(image.URI);

                        image.Filename = URI.Segments.Last();

                        // Binary Data
                        // image.BinaryData = webcli.DownloadData(image.URI);
                        // image.MD5 = GetMD5(image.BinaryData);
                        // image.Filesize = image.BinaryData.Length;

                        // var img = Image.FromStream(new MemoryStream(image.BinaryData));
                        // image.Height = img.Height;
                        // image.Width = img.Width;
                        // image.MIMEType = GetImageFormat(img);

                        // var thumbnail = GenerateThumbnail(img);
                        // image.ThumbnailBinaryData = ImageToByteArray(thumbnail);
                    }
                    catch (Exception e)
                    {
                        image.Error = e.Message;
                    }

                    image.EndProcessingOn = DateTime.Now;
                    result.Add(image);
                }
            }

            return result;
        }

        public static Image GenerateThumbnail(Image image, int width = 300, int height = 300)
        {
            if (image.Width <= 300 && image.Height <= 300)
            {
                return image;
            }

            if (image.Width > image.Height)
            {
                var ratio = (double)image.Height / image.Width;
                height = (int)(height * ratio);
            }
            else
            {
                var ratio = (double)image.Width / image.Height;
                width = (int)(width * ratio);
            }

            return new Bitmap(image, new Size(width, height));
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        public static byte[] ImageToByteArray(Image image, bool isThumbnail = false)
        {
            var format = isThumbnail ? ImageFormat.Jpeg : ImageFormat.Png;

            using (var ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }

        public static Bitmap ConvertByteArrayToBitmap(byte[] image)
        {
            using (var ms = new MemoryStream(image))
            {
                return new Bitmap(ms);
            }
        }

        public static Bitmap Base64StringToBitmap(string base64String)
        {
            base64String = base64String.Replace("data:image/png;base64,", String.Empty);
            var bitmapData = Convert.FromBase64String(base64String);
            var streamBitmap = new System.IO.MemoryStream(bitmapData);
            var bitmap = new Bitmap((Bitmap)Image.FromStream(streamBitmap));

            return bitmap;
        }
    }
}
