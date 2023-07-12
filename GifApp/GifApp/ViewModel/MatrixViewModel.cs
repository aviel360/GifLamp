using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using System.IO.Ports;
using System.Drawing.Drawing2D;

namespace GifApp
{
    public class MatrixFrame : BaseViewModel
    {
        public ObservableCollection<LedState> MatPixels
        {
            get { return m_MatPixels; }
            set { SetProperty(ref m_MatPixels, value); }
        }

        public int iFrameDelay
        {
            get { return m_iFrameDelay; }
            set { SetProperty(ref m_iFrameDelay, value); }
        }

        public MatrixFrame()
        {
            MatPixels = new ObservableCollection<LedState>();
        }

        public MatrixFrame(Color colorLed)
        {
            MatPixels = new ObservableCollection<LedState>();
            for (int iHeight = 0; iHeight < MatrixViewModel.MATRIX_HEIGHT; iHeight++)
            {
                for (int iWidth = 0; iWidth < MatrixViewModel.MATRIX_WIDTH; iWidth++)
                {
                    MatPixels.Add(new LedState(new SolidColorBrush(colorLed)));
                }
            }
            iFrameDelay = 0;
        }

        public MatrixFrame(MatrixFrame matFrame)
        {
            MatPixels = new ObservableCollection<LedState>();
            foreach(LedState cLed in matFrame.MatPixels)
            {
                MatPixels.Add(new LedState(cLed));
            }    


            iFrameDelay = matFrame.iFrameDelay;
        }

        #region Members

        protected ObservableCollection<LedState> m_MatPixels;
        protected int m_iFrameDelay;

        #endregion
    }

    public class MatrixViewModel : BaseViewModel
    {   

        #region Enums & Constants
        public enum DisplaySettings { NONE = 0, GIF = 1, IMAGE = 2, BUF = 3 };

        public const int MATRIX_HEIGHT = 32;
        public const int MATRIX_WIDTH = 32;

        #endregion

        #region Constructors
        public MatrixViewModel()
        {
            GetPorts();
            InitMatrix();
        }
        #endregion

        #region Properties
        public Array Ports
        {
            get { return m_arrPorts; }
            set { SetProperty(ref m_arrPorts, value); }
        }

        public MatrixFrame MatColorsCurrent
        {
            get { return m_matColorsCurrent; }
            set 
            {
                MatColorsCurrent.MatPixels = value.MatPixels;
                MatColorsCurrent.iFrameDelay = value.iFrameDelay;
            }
        }

        public string PortCurrent
        {
            get { return m_PortCurrent; }
            set { SetProperty(ref m_PortCurrent, value); }
        }

        public List<string> MatSizes
        {
            get { return m_MatSizes; }
            set { SetProperty(ref m_MatSizes, value); }
        }

        public List<string> MatFrame
        {
            get { return m_MatFrame; }
            set { SetProperty(ref m_MatFrame, value); }
        }

        public string MatSizeCurrent
        {
            get { return m_MatSizeCurrent; }
            set { SetProperty(ref m_MatSizeCurrent, value); }
        }

        public string MatFrameCurrent
        {
            get { return m_MatFrameCurrent; }
            set 
            {
                int iValue = int.Parse(value ?? 0.ToString());
                iValue = iValue < 0 ? 0 : iValue;
                MatColorsCurrent = MatColors[iValue];
                SetProperty(ref m_MatFrameCurrent, iValue.ToString()); 
            }
        }

        public Color BrushColorCurrent
        {
            get { return m_colorBrush; }
            set { SetProperty(ref m_colorBrush, value); }
        }

        public List<string> AnimationSpeed
        {
            get { return m_AnimationSpeed; }
            set { SetProperty(ref m_AnimationSpeed, value); }
        }
        public string AnimationSpeedCurrent
        {
            get { return m_AnimationSpeedCurrent; }
            set { SetProperty(ref m_AnimationSpeedCurrent, value); }
        }
        public ObservableCollection<MatrixFrame> MatColors
        {
            get { return m_matColors; }
            set { SetProperty(ref m_matColors, value); }
        }

        public DisplaySettings DisplaySetting
        {
            get { return m_DisplaySettingCurrent; }
            set { SetProperty(ref m_DisplaySettingCurrent, value); }   
        }
        #endregion


        #region Methods
        public void InitMatrix()
        {
            MatColors = new ObservableCollection<MatrixFrame>
            {
                new MatrixFrame()
            };
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    MatColors[0].MatPixels.Add(new LedState());
                }
            }
            MatColorsCurrent = MatColors[0];
        }
        public void GetPorts()
        {
            Ports = SerialPort.GetPortNames();
        }

        public void DisplayImage(int[] arrImage)
        {

            for (int i = 0; i < 16*16; i++)
            {
                System.Windows.Media.Color iColor = System.Windows.Media.Color.FromRgb((byte)((arrImage[i] >> 16) & 0xFF), (byte)((arrImage[i] >> 8) & 0xFF), (byte)(arrImage[i] & 0xFF));
                MatColors[0].MatPixels[i].Color  = new SolidColorBrush(iColor);
            }
        }

        public string GetDisplayString()
        {
            string strDisplay = "bin";
            switch(DisplaySetting)
            {
                case DisplaySettings.IMAGE: strDisplay = "jpg"; break;
                case DisplaySettings.GIF: strDisplay = "gif"; break;
            }
            return strDisplay;
        }
        #endregion

        #region Members
        protected List<string> m_MatSizes = new List<string> { "16x16", "16x32", "32x16", "32x32" };
        protected List<string> m_MatFrame = new List<string> { "0" };
        protected string m_MatFrameCurrent = "0";
        protected string m_MatSizeCurrent = string.Empty;
        protected List<string> m_AnimationSpeed = new List<string> { "x0.25", "x0.5", "x1", "x1.5", "x2", "x5", "x10" };
        protected string m_AnimationSpeedCurrent = "x1";
        protected Array m_arrPorts;
        protected string m_PortCurrent = string.Empty;
        protected int m_iFrameDelayCurrent;
        protected ObservableCollection<MatrixFrame> m_matColors = new ObservableCollection<MatrixFrame>();
        protected MatrixFrame m_matColorsCurrent = new MatrixFrame();
        protected DisplaySettings m_DisplaySettingCurrent = DisplaySettings.NONE;
        protected Color m_colorBrush;
        #endregion
    }
}
