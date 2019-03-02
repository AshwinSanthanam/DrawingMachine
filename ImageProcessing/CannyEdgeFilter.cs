using ImageProcessing.Helper;
using System;

namespace ImageProcessing
{
    public class CannyEdgeFilter
    {
        private readonly byte _lowerThreshold = 20;
        private readonly byte _upperThreshold = 50;
        public VectorPixel[][] VectorPixels { get; }

        public CannyEdgeFilter(VectorPixel[][] vectorPixels, byte lowerThreshold, byte upperThreshold)
        {
            _lowerThreshold = lowerThreshold;
            _upperThreshold = upperThreshold;
            VectorPixels = CopyVectorPixels(vectorPixels);
            NonMaximumSuppression();
            PerformThresholding();
            PerformHysteresisThresholding();
            SupressGrey();
        }

        private VectorPixel[][] CopyVectorPixels(VectorPixel[][] vectorPixels)
        {
            var newVectorPixels = new VectorPixel[vectorPixels.Length][];
            for (int y = 0; y < vectorPixels.Length; y++)
            {
                newVectorPixels[y] = new VectorPixel[vectorPixels[y].Length];
                for (int x = 0; x < vectorPixels[y].Length; x++)
                {
                    newVectorPixels[y][x] = new VectorPixel(vectorPixels[y][x]);
                }
            }
            return newVectorPixels;
        }

        private void SupressGrey()
        {
            for (int y = 0; y < VectorPixels.Length; y++)
            {
                for (int x = 0; x < VectorPixels[y].Length; x++)
                {
                    if (VectorPixels[y][x].NormalizedIntensity != 255)
                    {
                        VectorPixels[y][x].SuppressPixel();
                    }
                }
            }
        }

        private void NonMaximumSuppression()
        {
            for (int y = 1; y < VectorPixels.Length - 1; y++)
            {
                for (int x = 1; x < VectorPixels[0].Length - 1; x++)
                {
                    int[][] neighbours = GetPixelsAlongGradientDirection(VectorPixels[y][x].Direction, x, y);
                    try
                    {
                        if (VectorPixels[neighbours[0][1]][neighbours[0][0]].NormalizedIntensity > VectorPixels[y][x].NormalizedIntensity ||
                            VectorPixels[neighbours[1][1]][neighbours[1][0]].NormalizedIntensity > VectorPixels[y][x].NormalizedIntensity)
                        {
                            VectorPixels[y][x].SuppressPixel();
                        }
                        else
                        {
                            VectorPixels[neighbours[0][1]][neighbours[0][0]].SuppressPixel();
                            VectorPixels[neighbours[1][1]][neighbours[1][0]].SuppressPixel();
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        private int[][] GetPixelsAlongGradientDirection(double direction, int x, int y)
        {
            if(direction < 0)
            {
                direction += 360;
            }
            if (direction == 0 || direction == 180) 
            {
                return new int[][]
                {
                    new int[] {x - 1, y},
                    new int[] {x + 1, y}
                };
            }
            if (direction == 315 || direction == 135)
            {
                return new int[][]
                {
                    new int[] {x - 1, y + 1},
                    new int[] {x + 1, y - 1}
                };
            }
            if (direction == 90 || direction == 270)
            {
                return new int[][]
                {
                    new int[] {x, y - 1},
                    new int[] {x, y + 1}
                };
            }
            if (direction == 225 || direction == 45) 
            {
                return new int[][]
                {
                    new int[] {x - 1, y - 1},
                    new int[] {x + 1, y + 1}
                };
            }
            return null;
        }

        private int[][] GetPixelsAcrossGradientDirection(double direction, int x, int y)
        {
            if(direction < 0)
            {
                direction += 360;
            }
            if (direction == 90 || direction == 270)
            {
                return new int[][]
                {
                    new int[] {x - 1, y},
                    new int[] {x + 1, y}
                };
            }
            if (direction == 225 || direction == 45)
            {
                return new int[][]
                {
                    new int[] {x - 1, y + 1},
                    new int[] {x + 1, y - 1}
                };
            }
            if (direction == 0 || direction == 180)
            {
                return new int[][]
                {
                    new int[] {x, y - 1},
                    new int[] {x, y + 1}
                };
            }
            if (direction == 315 || direction == 135)
            {
                return new int[][]
                {
                    new int[] {x - 1, y - 1},
                    new int[] {x + 1, y + 1}
                };
            }
            return null;
        }

        private void PerformThresholding()
        {
            for (int y = 0; y < VectorPixels.Length; y++)
            {
                for (int x = 0; x < VectorPixels[y].Length; x++)
                {
                    if (VectorPixels[y][x].NormalizedIntensity <= _lowerThreshold)
                    {
                        VectorPixels[y][x].SuppressPixel();
                    }
                    else if (VectorPixels[y][x].NormalizedIntensity >= _upperThreshold)
                    {
                        VectorPixels[y][x].DisclosePixel();
                    }
                }
            }
        }

        private void PerformHysteresisThresholding()
        {
            for (int y = 0; y < VectorPixels.Length; y++)
            {
                for (int x = 0; x < VectorPixels[y].Length; x++)
                {
                    if (VectorPixels[y][x].NormalizedIntensity == 255)
                    {
                        int[][] neighbours = GetPixelsAcrossGradientDirection(VectorPixels[y][x].Direction, x, y);
                        TravelImage(neighbours[0][0], neighbours[0][1]);
                        TravelImage(neighbours[1][0], neighbours[1][1]);
                    }
                }
            }
        }

        private void TravelImage(int x, int y)
        {
            if (x < 0 || y < 0 || x > VectorPixels[0].Length - 1 || y > VectorPixels.Length - 1 || 
                VectorPixels[y][x].NormalizedIntensity == 255 || VectorPixels[y][x].NormalizedIntensity == 0)
            {
                return;
            }
            VectorPixels[y][x].DisclosePixel();
            TravelImage(x - 1, y);
            TravelImage(x + 1, y);
            TravelImage(x, y - 1);
            TravelImage(x, y + 1);
            TravelImage(x - 1, y - 1);
            TravelImage(x + 1, y + 1);
            TravelImage(x + 1, y - 1);
            TravelImage(x - 1, y + 1);
        }

    }
}
