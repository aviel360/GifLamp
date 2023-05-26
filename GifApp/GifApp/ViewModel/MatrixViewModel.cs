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

namespace GifApp
{
    public class MatrixViewModel : BaseViewModel
    {
        #region Constructors
        public MatrixViewModel()
        {
            GetPorts();
            InitMatrix();
        }
        #endregion

        #region Properties
        public ObservableCollection<string> Ports
        {
            get { return m_arrPorts; }
            set { SetProperty(ref m_arrPorts, value); }
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

        public string MatSizeCurrent
        {
            get { return m_MatSizeCurrent; }
            set { SetProperty(ref m_MatSizeCurrent, value); }
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
        public ObservableCollection<LedState> MatColors
        {
            get { return m_matColors; }
            set { SetProperty(ref m_matColors, value); }
        }
        #endregion


        #region Methods
        public void InitMatrix()
        {
            MatColors = new ObservableCollection<LedState>();
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    MatColors.Add(new LedState());
                }
            }
        }
        public void GetPorts()
        {
            Array arrPorts = SerialPort.GetPortNames();
            foreach (string port in arrPorts)
            {
                Ports.Add(port);
            }
        }

        public void DisplayImage(int[] arrImage)
        {

            for (int i = 0; i < 16*16; i++)
            {
                System.Windows.Media.Color iColor = System.Windows.Media.Color.FromRgb((byte)((arrImage[i] >> 16) & 0xFF), (byte)((arrImage[i] >> 8) & 0xFF), (byte)(arrImage[i] & 0xFF));
                MatColors[i].Color  = new SolidColorBrush(iColor);
            }
        }
        #endregion

        #region Members
        protected List<string> m_MatSizes = new List<string> { "16x16", "16x32", "32x16", "32x32" };
        protected string m_MatSizeCurrent = string.Empty;
        protected List<string> m_AnimationSpeed = new List<string> { "x0.5", "x1", "x1.5", "x2" };
        protected string m_AnimationSpeedCurrent = "x1";
        protected ObservableCollection<string> m_arrPorts = new ObservableCollection<string>();
        protected string m_PortCurrent = string.Empty;
        protected ObservableCollection<LedState> m_matColors = new ObservableCollection<LedState>();
        #endregion
    }
}
