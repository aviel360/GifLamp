using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GifApp
{
    public class LedState
    {
        public Brush Color { get; set; }
        public double Brightness { get; set; } = 1.0;

        private static Random _random = new Random();

        public LedState()
        {
            Color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF0000") ?? Brushes.White);
        }
    }
}
