using ImageProcessing.Helper;

namespace ImageProcessing
{
    public class SobelOperator
    {
        public VectorPixel[][] VectorPixels { get; }

        public SobelOperator(byte[][] image)
        {
            VectorPixels = new VectorPixel[image.Length - 2][];
            PopulateVectorPixels(image);
        }

        private void PopulateVectorPixels(byte[][] image)
        {
            for (int y = 1; y < image.Length - 1; y++)
            {
                VectorPixels[y - 1] = new VectorPixel[image[y].Length - 2];
                for (int x = 1; x < image[y].Length - 1; x++)
                {
                    int xGradient = FindXGradient(image, x, y);
                    int yGradient = FindYGradient(image, x, y);
                    VectorPixels[y - 1][x - 1] = new VectorPixel(xGradient, yGradient);
                }
            }
        }

        private int FindXGradient(byte[][] image, int x, int y)
        {
            int left = Util.GetPixel(image, x - 1, y);
            int right = Util.GetPixel(image, x + 1, y);
            int topLeft = Util.GetPixel(image, x - 1, y - 1);
            int topRight = Util.GetPixel(image, x + 1, y - 1);
            int bottomLeft = Util.GetPixel(image, x - 1, y + 1);
            int bottomRight = Util.GetPixel(image, x + 1, y + 1);
            return (right - left) * 2 + topRight - topLeft + bottomRight - bottomLeft;
        }

        private int FindYGradient(byte[][] image, int x, int y)
        {
            int top = Util.GetPixel(image, x, y - 1);
            int bottom = Util.GetPixel(image, x, y + 1);
            int topLeft = Util.GetPixel(image, x - 1, y - 1);
            int topRight = Util.GetPixel(image, x + 1, y - 1);
            int bottomLeft = Util.GetPixel(image, x - 1, y + 1);
            int bottomRight = Util.GetPixel(image, x + 1, y + 1);
            return (bottom - top) * 2 + bottomRight - topRight + bottomLeft - topLeft;
        }
    }
}
