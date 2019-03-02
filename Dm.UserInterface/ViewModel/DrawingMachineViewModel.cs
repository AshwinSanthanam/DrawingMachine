using ImageProcessing;
using ImageProcessing.Helper;
using System.IO;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Dm.UserInterface.ViewModel
{
    class DrawingMachineViewModel : ViewModelBase
    {
        private string _image;
        private byte _lowerThreshold;
        private byte _upperThreshold;
        private readonly VectorPixel[][] _vectorPixels;

        public string Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                NotifyPropertyChange("Image");
            }
        }

        public byte UpperThreshold
        {
            get
            {
                return _upperThreshold;
            }
            set
            {
                _upperThreshold = value;
                NotifyPropertyChange("UpperThreshold");
                ApplyCannyEdgeFilter();
            }
        }

        public byte LowerThreshold
        {
            get
            {
                return _lowerThreshold ;
            }
            set
            {
                _lowerThreshold = value;
                NotifyPropertyChange("LowerThreshold");
                ApplyCannyEdgeFilter();
            }
        }

        public ICommand ButtonClickCommand { get; set; }

        public DrawingMachineViewModel()
        {
            ButtonClickCommand = new RelayCommand(OnButtonClick, CanExecute);
            _vectorPixels = InitilizeVectorPixels();
        }

        private VectorPixel[][] InitilizeVectorPixels()
        {
            var image = Util.GetGreyScaleImage(@"D:\Image Resource\21.png");
            GaussianBlur gaussianBlur = new GaussianBlur(image);
            var sobelOperator = new SobelOperator(gaussianBlur.GaussianBlurredImage);
            VectorPixel.NormalizeIntensity(sobelOperator.VectorPixels);
            return sobelOperator.VectorPixels;
        }

        private void ApplyCannyEdgeFilter()
        {
            CannyEdgeFilter cannyEdgeFilter = new CannyEdgeFilter(_vectorPixels, LowerThreshold, UpperThreshold);
            string imgLoc;
            if (!string.IsNullOrEmpty(Image) && Image.Equals(@"D:\Sobel1.png"))
            {
                imgLoc = @"D:\Sobel2.png";
            }
            else
            {
                imgLoc = @"D:\Sobel1.png";
            }
            Util.SaveImage(imgLoc, VectorPixel.GetByteArray(cannyEdgeFilter.VectorPixels));
            Image = imgLoc;
        }

        private void OnButtonClick(object parameter)
        {
            ApplyCannyEdgeFilter();
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
