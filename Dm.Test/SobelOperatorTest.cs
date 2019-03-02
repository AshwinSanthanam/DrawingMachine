using System;
using System.Diagnostics;
using ImageProcessing;
using ImageProcessing.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dm.Test
{
    [TestClass]
    public class SobelOperatorTest
    {
        [TestMethod]
        public void SobelTest()
        {
            var image = Util.GetGreyScaleImage(@"D:\Image Resource\7.png");
            GaussianBlur gaussianBlur = new GaussianBlur(image);
            SobelOperator sobelOperator = new SobelOperator(gaussianBlur.GaussianBlurredImage);
            VectorPixel.NormalizeIntensity(sobelOperator.VectorPixels);
            Util.SaveImage(@"D:\Sobel.png", VectorPixel.GetByteArray(sobelOperator.VectorPixels));
            CannyEdgeFilter cannyEdgeFilter = new CannyEdgeFilter(sobelOperator.VectorPixels, 20, 50);
            Util.SaveImage(@"D:\Canny.png", VectorPixel.GetByteArray(cannyEdgeFilter.VectorPixels));
            Process.Start(@"D:\Canny.png");
        }
    }
}
