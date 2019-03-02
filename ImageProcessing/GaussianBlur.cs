using ImageProcessing.Helper;

namespace ImageProcessing
{
    public class GaussianBlur
    {
        private readonly byte[][] _originalImage;
        private byte[][] _resultImage;

        public GaussianBlur(byte[][] inputImage)
        {
            _originalImage = inputImage;
        }

        private byte GetGaussianBlurForPixel(int x, int y)
        {
            int up = Util.GetPixel(_originalImage, x, y - 1);
            int down = Util.GetPixel(_originalImage, x, y + 1);
            int left = Util.GetPixel(_originalImage, x - 1, y);
            int right = Util.GetPixel(_originalImage, x + 1, y);
            int upLeft = Util.GetPixel(_originalImage, x - 1, y - 1);
            int upRight = Util.GetPixel(_originalImage, x + 1, y - 1);
            int downLeft = Util.GetPixel(_originalImage, x - 1, y + 1);
            int downRight = Util.GetPixel(_originalImage, x + 1, y + 1);
            int current = Util.GetPixel(_originalImage, x, y);
            return Util.GetClosestByte(((up + down + left + right) * 2 + upLeft + upRight + downLeft + downRight + current * 4) / 16.0);
        }

        public byte[][] GaussianBlurredImage
        {
            get
            {
                if(_resultImage != null)
                {
                    return _resultImage;
                }
                _resultImage = new byte[_originalImage.Length][];
                for (int i = 0; i < _originalImage.Length; i++)
                {
                    _resultImage[i] = new byte[_originalImage[i].Length];
                }
                for (int y = 0; y < _originalImage.Length; y++)
                {
                    for (int x = 0; x < _originalImage[y].Length; x++)
                    {
                        _resultImage[y][x] = GetGaussianBlurForPixel(x, y);
                    }
                }
                return _resultImage;
            }
        }
    }
}
