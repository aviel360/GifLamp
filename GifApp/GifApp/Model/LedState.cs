using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
#nullable enable

namespace GifApp
{
    public class LedState : BaseViewModel
    {
        private Brush? m_ColorCurrent;
        public Brush Color
        {
            get { return m_ColorCurrent; }
            set { SetProperty(ref m_ColorCurrent, value); }
        }
        public double Brightness { get; set; } = 1.0;

        private static Random _random = new Random();

        public LedState(Brush? brush = null)
        {
            Color = brush ?? Brushes.Black;
        }
    }
}
