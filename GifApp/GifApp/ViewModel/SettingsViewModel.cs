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
    public class SettingsViewModel : BaseViewModel
    {
        protected List<string> m_MatSizes = new List<string> { "16x16", "16x32", "32x16", "32x32" };
        protected string m_MatSizeCurrent = string.Empty;
        protected List<string> m_AnimationSpeed = new List<string> { "x0.5", "x1", "x1.5", "x2" };
        protected string m_AnimationSpeedCurrent = "x1";
        protected ObservableCollection<string> m_arrPorts = new ObservableCollection<string>();
        protected string m_PortCurrent = string.Empty;

        public ObservableCollection<string> Ports
        {
            get { return m_arrPorts; }
            set { SetProperty(ref m_arrPorts, value);}
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
        public SettingsViewModel() 
        {
            Array arrPorts = SerialPort.GetPortNames();
            foreach(string port in arrPorts)
            {
                Ports.Add(port);
            }
        }
    }
}
