using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;

namespace GifApp
{
    public class MatrixViewModel : BaseViewModel
    {

        protected ObservableCollection<LedState> m_matColors = new ObservableCollection<LedState>();

        public ObservableCollection<LedState> MatColors
        {
            get { return m_matColors; }
            set { SetProperty(ref m_matColors, value); }
        }

        public MatrixViewModel() 
        {
            MatColors = new ObservableCollection<LedState>();
            for(int i = 0; i < 16;  i++) 
            { 
                for(int j = 0; j < 16; j++) 
                {
                    MatColors.Add(new LedState());
                }
            }
        }
    }
}
