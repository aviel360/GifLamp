using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
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
using Microsoft.Win32;
using System.IO.Pipes;
using System.Threading;

namespace GifApp
{
    /// <summary>
    /// Interaction logic for MatrixView.xaml
    /// </summary>
    public partial class MatrixView : UserControl
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

        protected string m_colorCurrentBrush;
        protected string m_imageLocation;
        protected SerialPort m_serialPort;
        protected bool m_bIsConnected = false;

        public MatrixView()
        {
            InitializeComponent();
            DataContext = new MatrixViewModel();
            m_colorCurrentBrush = "#FFFFFF";

            Task.Run(RunTask);
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
        private void UploadCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*.jpg| PNG files(*.png)| All Files(*.*)|(*.*)|";
                if (dialog.ShowDialog() ?? false)
                {
                    m_imageLocation = dialog.FileName;
                }

            }
            catch (Exception)
            {

            }
        }
        private void ConnectCommand(object sender, RoutedEventArgs e)
        {
            if(m_bIsConnected)
            {
                m_serialPort.Close();
                ButtonConnect.Content = "Connect";
                PortsComboBox.IsEnabled = true;
                m_bIsConnected = false;
            }
            else
            {
                string? selectedPort = PortsComboBox.SelectedItem?.ToString();
                if (!String.IsNullOrEmpty(selectedPort))
                {
                    m_serialPort = new SerialPort(selectedPort, 921600, Parity.None);
                    m_serialPort.Open();
                    ButtonConnect.Content = "Disconnect";
                    PortsComboBox.IsEnabled = false;
                    m_bIsConnected = true;
                }
            }
        }

        static void RunTask()
        {
            while (true)
            {
                // Do some work here
                //SendData();

                // Delay for 50 milliseconds
                Thread.Sleep(50);
            }
        }

        private void SendCommand(object sender, RoutedEventArgs e)
        {
            byte[] bytes = new byte[16 * 16 * 3]; // Assuming RGB format

            // Loop through the matrix and convert each hex value to bytes
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    int colorValue = arrPokeball[x, y]; // Get the color value from the matrix

                    // Convert the color value to bytes
                    byte r = (byte)((colorValue >> 16) & 0xFF); // Red component
                    byte g = (byte)((colorValue >> 8) & 0xFF); // Green component
                    byte b = (byte)(colorValue & 0xFF); // Blue component

                    // Copy the color bytes to the main byte array
                    int index = (y * 16 + x) * 3;
                    bytes[index] = r;
                    bytes[index + 1] = g;
                    bytes[index + 2] = b;
                }
            }
            m_serialPort.Write(bytes, 0, bytes.Length);
        }
        private void SendData()
        {
            //open the file using file stream
            FileStream fileStream = new FileStream(@"C:\Workspace\GifLamp\GifApp\GifApp\Resources\xw.bin", FileMode.Open, FileAccess.Read, FileShare.Read);

            //store the open file as binary
            BinaryReader binary = new BinaryReader(fileStream, Encoding.GetEncoding(28591));

            //at this point I write to the port
            m_serialPort.Write(binary.ReadBytes((int)fileStream.Length), 0, (int)fileStream.Length);
        }
    }
}
