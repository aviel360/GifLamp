using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GifApp
{
    /// <summary>
    /// Interaction logic for Matrix.xaml
    /// </summary>
    public partial class Matrix : UserControl
    {
        protected string m_colorCurrentBrush;
        public Matrix()
        {
            InitializeComponent();
            DataContext = new MatrixViewModel();
            m_colorCurrentBrush = "#FFFFFF";
    }
    private void ClrPcker_Background_SelectedColorChanged(object sender, EventArgs e)
        {
            m_colorCurrentBrush = ClrPcker_Background.SelectedColor.ToString() ?? m_colorCurrentBrush;
        }
        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle? clickedRectangle = e.OriginalSource as Rectangle;
            if (clickedRectangle != null)
            {
                // Do something with the clicked Rectangle
                clickedRectangle.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom(m_colorCurrentBrush) ?? clickedRectangle.Fill);
            }
        }
    }
}
