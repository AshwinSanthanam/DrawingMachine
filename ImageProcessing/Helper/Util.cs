using System;
using System.Drawing;

namespace ImageProcessing.Helper
{
    public static class Util
    {
        public static byte GetClosestByte(double value)
        {
            int valueAsInt = Convert.ToInt32(value);
            return (byte)(valueAsInt + (value - valueAsInt > 0.5 ? 1 : 0));
        }

        public static byte GetPixel(byte[][] image, int x, int y)
        {
            try
            {
                return image[y][x];
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static void SaveImage(string fullPath, byte[][] image)
        {
            Bitmap bitmap = new Bitmap(image[0].Length, image.Length);
            for (int y = 0; y < image.Length; y++)
            {
                for (int x = 0; x < image[y].Length; x++)
                {
                    byte color = image[y][x];
                    bitmap.SetPixel(x, y, Color.FromArgb(color, color, color));
                }
            }
            bitmap.Save(fullPath);
        }

        public static short RoundOff(short lowerValue, short upperValue, double value)
        {
            double lowerDifference = value - lowerValue;
            double upperDifference = upperValue - value;
            return lowerDifference < upperDifference ? lowerValue : upperValue;
        }

        public static byte[][] GetGreyScaleImage(string fullPath)
        {
            Bitmap image = new Bitmap(fullPath);
            byte[][] greyscaleImage = new byte[image.Height][];
            for (int y = 0; y < image.Height; y++)
            {
                greyscaleImage[y] = new byte[image.Width];
                for (int x = 0; x < image.Width; x++)
                {
                    Color currentColor = image.GetPixel(x, y);
                    byte color = (byte)(currentColor.R * 0.3 + currentColor.G * 0.59 + currentColor.B * 0.11);
                    greyscaleImage[y][x] = color;
                }
            }
            return greyscaleImage;
        }
    }
}
