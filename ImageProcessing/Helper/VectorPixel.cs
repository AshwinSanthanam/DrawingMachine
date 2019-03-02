using System;

namespace ImageProcessing.Helper
{
    public class VectorPixel
    {
        public byte NormalizedIntensity { get; private set; }
        public short Direction { get; private set; }
        public double ResultantIntensity { get; private set; }

        public VectorPixel(int xGradient, int yGradient)
        {
            ResultantIntensity = FindResultantIntensity(xGradient, yGradient);
            Direction = FindGrdientDirection(xGradient, yGradient);
        }

        public VectorPixel(VectorPixel vectorPixel)
        {
            NormalizedIntensity = vectorPixel.NormalizedIntensity;
            ResultantIntensity = vectorPixel.ResultantIntensity;
            Direction = vectorPixel.Direction;
        }

        public void SuppressPixel()
        {
            NormalizedIntensity = 0;
        }

        public void DisclosePixel()
        {
            NormalizedIntensity = 255;
        }

        public static void NormalizeIntensity(VectorPixel[][] vectorPixels)
        {
            GetMinMaxIntensity(vectorPixels, out double minInensity, out double maxIntensity);
            double intensityRange = maxIntensity - minInensity;
            for (int y = 0; y < vectorPixels.Length; y++)
            {
                for (int x = 0; x < vectorPixels[y].Length; x++)
                {
                    double normalizationFactor = ((vectorPixels[y][x].ResultantIntensity - minInensity) / intensityRange);
                    vectorPixels[y][x].NormalizedIntensity = Util.GetClosestByte(255 * normalizationFactor);
                }
            }
        }

        public static byte[][] GetByteArray(VectorPixel[][] vectorPixels)
        {
            byte[][] image = new byte[vectorPixels.Length][];
            for (int y = 0; y < image.Length; y++)
            {
                image[y] = new byte[vectorPixels[y].Length];
                for (int x = 0; x < image[y].Length; x++)
                {
                    image[y][x] = vectorPixels[y][x].NormalizedIntensity;
                }
            }
            return image;
        }

        private static double FindResultantIntensity(int xGradient, int yGradient)
        {
            return Math.Sqrt(xGradient * xGradient + yGradient * yGradient);
        }

        private static short FindGrdientDirection(int xGradient, int yGradient)
        {
            if(xGradient == 0)
            {
                if(yGradient < 0)
                {
                    return -90;
                }
                else
                {
                    return 90;
                }
            }
            double gradientDirection = Math.Atan2(yGradient, xGradient) * (180 / Math.PI);
            if (gradientDirection >= 0 && gradientDirection <= 45)
            {
                return Util.RoundOff(0, 45, gradientDirection);
            }
            else if (gradientDirection >= 45 && gradientDirection <= 90)
            {
                return Util.RoundOff(45, 90, gradientDirection);
            }
            else if (gradientDirection >= 90 && gradientDirection <= 135)
            {
                return Util.RoundOff(90, 135, gradientDirection);
            }
            else if (gradientDirection >= 135 && gradientDirection <= 180)
            {
                return Util.RoundOff(135, 180, gradientDirection);
            }
            else if (gradientDirection >= -45 && gradientDirection <= 0)
            {
                return Util.RoundOff(-45, 0, gradientDirection);
            }
            else if (gradientDirection >= -90 && gradientDirection <= -45)
            {
                return Util.RoundOff(-90, -45, gradientDirection);
            }
            else if (gradientDirection >= -135 && gradientDirection <= -90)
            {
                return Util.RoundOff(-135, -90, gradientDirection);
            }
            else if (gradientDirection >= -135 && gradientDirection <= -90)
            {
                return Util.RoundOff(-135, -90, gradientDirection);
            }
            else if (gradientDirection >= -180 && gradientDirection <= -135)
            {
                return Util.RoundOff(-180, -135, gradientDirection);
            }
            else
            {
                throw new Exception("Invalid xGradient and/or yGradient");
            }
        }

        private static void GetMinMaxIntensity(VectorPixel[][] vectorPixels, out double minIntensity, out double maxIntensity)
        {
            minIntensity = double.MaxValue;
            maxIntensity = double.MinValue;
            for (int y = 0; y < vectorPixels.Length; y++)
            {
                for (int x = 0; x < vectorPixels[y].Length; x++)
                {
                    minIntensity = minIntensity > vectorPixels[y][x].ResultantIntensity ? vectorPixels[y][x].ResultantIntensity : minIntensity;
                    maxIntensity = maxIntensity < vectorPixels[y][x].ResultantIntensity ? vectorPixels[y][x].ResultantIntensity : maxIntensity;
                }
            }
        }


    }
}
