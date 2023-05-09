﻿using System;
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
    public class MatrixViewModel : BaseViewModel
    {
        protected int[] arrPokeball =
        {
            0xd0d2d4, 0xdedfe0, 0xdbdcde, 0xdbdcde, 0xd0d2d4, 0x1e1d1d, 0x030202, 0x100708, 0x100708, 0x030202, 0x222021, 0xd2d4d6, 0xdbdcde, 0xdedfe0, 0xdad9dc, 0xa69da1,
            0xdedfe0, 0xf0f1f3, 0xe6e7e9, 0x403f40, 0x242323, 0xc12328, 0xcf262b, 0xcf262b, 0xcf262b, 0xcf262b, 0xc12328, 0x222021, 0x454646, 0xe6e7e9, 0xf0f1f3, 0xe2e1e3,
            0xdbdcde, 0xe6e7e9, 0x3b3738, 0x98000b, 0xd21f25, 0xf62e34, 0xfc3137, 0xfc3137, 0xfc3137, 0xfc3137, 0xf62e34, 0xd21f25, 0xaa1c20, 0x403f40, 0xeaebed, 0xdad9dc,
            0xdedfe0, 0x454646, 0x670003, 0xd70405, 0xf62e34, 0xf12e34, 0xf12e34, 0xf12e34, 0xf12e34, 0xf12e34, 0xf12e34, 0xfc3137, 0xfc3137, 0xaa1c20, 0x454646, 0xe2e1e3,
            0xdad9dc, 0x2a292a, 0x7e0006, 0xd70405, 0xee2a30, 0xf12e34, 0xf12e34, 0xf12e34, 0xf12e34, 0xf12e34, 0xf12e34, 0xee2a30, 0xfc3137, 0xc12328, 0x2a292a, 0xdbdcde,
            0x2a292a, 0x71020d, 0xc90207, 0xd70405, 0xee2a30, 0xf12e34, 0xf12e34, 0xf12e34, 0xf12e34, 0xf12e34, 0xf12e34, 0xf12e34, 0xf12e34, 0xf62e34, 0xaa1c20, 0x303132,
            0x111111, 0x7e010e, 0xd70405, 0xd70405, 0xd90708, 0xd90708, 0xd90708, 0xd90708, 0xd90708, 0xdb0b0d, 0xf12e34, 0xf12e34, 0xee2a30, 0xfc3137, 0xc12328, 0x111111,
            0x161616, 0x71020d, 0xa1010c, 0xd00305, 0xd70405, 0xd70405, 0xd00305, 0xa1010c, 0x98000b, 0xa1010c, 0xd90708, 0xd90708, 0xd90708, 0xe1090a, 0xa1010c, 0x161616,
            0x161616, 0x100708, 0x7e010e, 0x98000b, 0x92000c, 0x98000b, 0x92000c, 0x190204, 0x030202, 0x190204, 0x92000c, 0x98000b, 0x98000b, 0x84020d, 0x190204, 0x161616,
            0x111111, 0x9ba0a1, 0x3b3738, 0x100708, 0x1a0d10, 0x1a0d10, 0x1a0d10, 0x030202, 0x070909, 0x030202, 0x1a0d10, 0x1a0d10, 0x100708, 0x3b3738, 0x9ba0a1, 0x111111,
            0x1e1d1d, 0x161616, 0xadb1b2, 0xcbcdcf, 0xc4c7c9, 0xcbcdcf, 0xb5b7b8, 0x242323, 0xa9a9aa, 0x242323, 0xbcbdbe, 0xc4c7c9, 0xcbcdcf, 0xa9a9aa, 0x161616, 0x222021,
            0xcbcdcf, 0x222021, 0x111111, 0x242323, 0x242323, 0x242323, 0x222021, 0x030202, 0x111111, 0x030202, 0x242323, 0x242323, 0x2a292a, 0x161616, 0x242323, 0xcbcdcf,
            0xe2e1e3, 0x3b3738, 0x454646, 0x767676, 0x989999, 0xa69da1, 0x989999, 0x161616, 0x030202, 0x1e1d1d, 0xc4c7c9, 0xd0d2d4, 0xd6d7d9, 0x989999, 0x3b3738, 0xe2e1e3,
            0xdbdcde, 0xdbdcde, 0x303132, 0x656565, 0x818181, 0x818181, 0xb5b7b8, 0xa9a9aa, 0xa9a9aa, 0xa9a9aa, 0xbcbdbe, 0xbcbdbe, 0x989999, 0x403f40, 0xdedfe0, 0xdbdcde,
            0xdbdcde, 0xf0f1f3, 0xd6d7d9, 0x303132, 0x161616, 0x656565, 0x767676, 0x767676, 0x767676, 0x767676, 0x656565, 0x161616, 0x3b3738, 0xd6d7d9, 0xf0f1f3, 0xdbdcde,
            0xdbdcde, 0xeaebed, 0xeaebed, 0xdad9dc, 0xcbcdcf, 0x1e1d1d, 0x030202, 0x070909, 0x070909, 0x030202, 0x222021, 0xcbcdcf, 0xd6d7d9, 0xeaebed, 0xeaebed, 0xdbdcde
        };

        protected ObservableCollection<LedState> m_matColors = new ObservableCollection<LedState>();

        public ObservableCollection<LedState> MatColors
        {
            get { return m_matColors; }
            set { SetProperty(ref m_matColors, value); }
        }

        public MatrixViewModel() 
        {
            MatColors = new ObservableCollection<LedState>();
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    MatColors.Add(new LedState());
                }
            }
            DisplayImage(arrPokeball);
        }

        public void DisplayImage(int[] arrImage)
        {
            for (int i = 0; i < 16*16; i++)
            {
                System.Windows.Media.Color iColor = System.Windows.Media.Color.FromRgb((byte)((arrImage[i] >> 16) & 0xFF), (byte)((arrImage[i] >> 8) & 0xFF), (byte)(arrImage[i] & 0xFF));
                MatColors[i].Color  = new SolidColorBrush(iColor);
            }
        }
    }
}
