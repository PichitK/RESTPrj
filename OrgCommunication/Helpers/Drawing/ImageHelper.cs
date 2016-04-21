using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace OrgCommunication.Helpers.Drawing
{
    public class ImageHelper
    {
        public enum ResizeMode
        {
            KeepAspectRatio = 0,
            Crop,
            Fixed
        }

        public static string GetImageFormat(System.Drawing.Image image)
        {
            ImageFormat bmpFormat = image.RawFormat;
            string strFormat = null;

            if (bmpFormat.Equals(ImageFormat.Bmp)) strFormat = "BMP";
            else if (bmpFormat.Equals(ImageFormat.Emf)) strFormat = "EMF";
            else if (bmpFormat.Equals(ImageFormat.Exif)) strFormat = "EXIF";
            else if (bmpFormat.Equals(ImageFormat.Gif)) strFormat = "GIF";
            else if (bmpFormat.Equals(ImageFormat.Icon)) strFormat = "Icon";
            else if (bmpFormat.Equals(ImageFormat.Jpeg)) strFormat = "JPEG";
            else if (bmpFormat.Equals(ImageFormat.MemoryBmp)) strFormat = "MemoryBMP";
            else if (bmpFormat.Equals(ImageFormat.Png)) strFormat = "PNG";
            else if (bmpFormat.Equals(ImageFormat.Tiff)) strFormat = "TIFF";
            else if (bmpFormat.Equals(ImageFormat.Wmf)) strFormat = "WMF";

            return strFormat;
        }

        public static Image ReSize(Image image, int? width, int? height, ResizeMode mode)
        {
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;
            int destWidth = 0;
            int destHeight = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            if (!width.HasValue)
                width = sourceWidth;

            if (!height.HasValue)
                height = sourceHeight;

            if ((width == sourceWidth) && (height == sourceHeight))
                return image;

            if (mode == ResizeMode.Fixed)
            {
                destWidth = width.Value;
                destHeight = height.Value;
            }
            else
            {
                nPercentW = ((float)width / (float)sourceWidth);
                nPercentH = ((float)height / (float)sourceHeight);

                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentH;

                    if (mode == ResizeMode.Crop)
                        destX = System.Convert.ToInt16((width - (sourceWidth * nPercent)) / 2);
                }
                else
                {
                    nPercent = nPercentW;

                    if (mode == ResizeMode.Crop)
                        destY = System.Convert.ToInt16((height - (sourceHeight * nPercent)) / 2);
                }

                destWidth = (int)(sourceWidth * nPercent);
                destHeight = (int)(sourceHeight * nPercent);
            }

            Bitmap bmPhoto = new Bitmap(width.Value, height.Value, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
            {
                //grPhoto.Clear(Color.Red);
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

                grPhoto.DrawImage(image,
                    new Rectangle(destX, destY, destWidth, destHeight),
                    new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);
            }
            
            return bmPhoto;
        }

        public static byte[] ImageToByteArray(Image image, ImageFormat format)
        {
            byte[] bytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                bytes = ms.ToArray();
            }

            return bytes;
        }
    }
}