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

namespace GifApp
{
    public class SettingsViewModel : BaseViewModel
    {
        protected List<string> m_MatSizes = new List<string> { "16x16", "16x32", "32x16", "32x32" };
        protected string m_MatSizeCurrent = string.Empty;
        protected List<string> m_AnimationSpeed = new List<string> { "x0.5", "x1", "x1.5", "x2" };
        protected string m_AnimationSpeedCurrent = "x1";


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

        }

    }
}
