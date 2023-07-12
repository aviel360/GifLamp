using ImageMagick;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xaml;
using static GifApp.MatrixViewModel;

#nullable enable

namespace GifApp
{
    /// <summary>
    /// Interaction logic for MatrixView.xaml
    /// </summary>
    public partial class MatrixView : UserControl
    {


        protected SerialPort? m_serialPort;
        protected bool m_bIsConnected = false;
        protected MatrixViewModel m_matViewModel;

        public MatrixView()
        {
            InitializeComponent();
            m_matViewModel = new MatrixViewModel();
            DataContext = m_matViewModel;
            //m_colorCurrentBrush = "#FFFFFF";
            m_matViewModel.BrushColorCurrent = Colors.White;
            Task.Run(RunTask);
        }

        private void ClrPcker_Background_SelectedColorChanged(object sender, EventArgs e)
        {
            m_matViewModel.BrushColorCurrent = ClrPcker_Background.SelectedColor ?? m_matViewModel.BrushColorCurrent;
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle? clickedRectangle = e.OriginalSource as Rectangle;
            if (clickedRectangle != null)
            {
                // Do something with the clicked Rectangle
                clickedRectangle.Fill = (new SolidColorBrush(m_matViewModel.BrushColorCurrent));
                //clickedRectangle.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom(m_colorCurrentBrush) ?? clickedRectangle.Fill);
            }
        }
        private void Rectangle_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle? clickedRectangle = e.OriginalSource as Rectangle;
            if (clickedRectangle != null)
            {
                // Do something with the clicked Rectangle
                //m_colorCurrentBrush = clickedRectangle.Fill.ToString();
                SolidColorBrush rgbValue = clickedRectangle.Fill as SolidColorBrush ?? new SolidColorBrush(Colors.White);
                m_matViewModel.BrushColorCurrent = rgbValue.Color;
            }
        }
        private void PlusCommand(object sender, RoutedEventArgs e)
        {
            int iCurrentMat = int.Parse(m_matViewModel.MatFrameCurrent);
            MatrixFrame matFrame = CopyCheckbox.IsChecked ?? false ? new MatrixFrame(m_matViewModel.MatColors[iCurrentMat]) : new MatrixFrame(Colors.Black);
            matFrame.iFrameDelay = m_matViewModel.MatColors[iCurrentMat].iFrameDelay;

            ObservableCollection<MatrixFrame> matColorsNew = new ObservableCollection<MatrixFrame>(m_matViewModel.MatColors);
            matColorsNew.Insert(iCurrentMat + 1, matFrame);
            m_matViewModel.MatColors = matColorsNew;

            List<string> matFrameCount = new List<string>(m_matViewModel.MatFrame);
            matFrameCount.Add((m_matViewModel.MatFrame.Count).ToString());
            m_matViewModel.MatFrame = matFrameCount;

            m_matViewModel.MatColorsCurrent = m_matViewModel.MatColors[iCurrentMat + 1];
            m_matViewModel.MatFrameCurrent = (iCurrentMat + 1).ToString();
        }

        private void MinusCommand(object sender, RoutedEventArgs e)
        {
            int iCurrentMat = int.Parse(m_matViewModel.MatFrameCurrent);
            iCurrentMat = iCurrentMat < 0 ? 0 : iCurrentMat;
            int nextMat = iCurrentMat == m_matViewModel.MatColors.Count - 1 ? iCurrentMat - 1 : iCurrentMat;
            if(m_matViewModel.MatFrame.Count > 1)
            {
                ObservableCollection<MatrixFrame> matColorsNew = new ObservableCollection<MatrixFrame>(m_matViewModel.MatColors);
                matColorsNew.RemoveAt(iCurrentMat);
                m_matViewModel.MatColors = matColorsNew;

                m_matViewModel.MatFrameCurrent = (nextMat).ToString();
                List<string> matFrameCount = new List<string>(m_matViewModel.MatFrame);
                matFrameCount.RemoveAt(m_matViewModel.MatFrame.Count - 1);
                m_matViewModel.MatFrame = matFrameCount;

                m_matViewModel.MatColorsCurrent = m_matViewModel.MatColors[nextMat];
            }
            else
            {
                m_matViewModel.MatColors[0] = new MatrixFrame(Colors.Black);
                m_matViewModel.MatFrameCurrent = "0";
            }
        }

        private void UploadCommand(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.buf, *.bmp, *.jpg, *.jpeg, *.gif, *.png)|*.buf;*.bmp;*.jpg;*.jpeg;*.png;*.gif|All files (*.*)|*.*";

            if (dialog.ShowDialog() ?? false)
            {
                string strImagePath = dialog.FileName;
                string strExt = System.IO.Path.GetExtension(strImagePath);
                SetDisplay(strExt);
                if(m_matViewModel.DisplaySetting == DisplaySettings.BUF)
                {
                    using (StreamReader reader = new StreamReader(strImagePath))
                    {
                        m_matViewModel.MatColors = (ObservableCollection<MatrixFrame>)XamlServices.Load(reader);
                        m_matViewModel.MatFrameCurrent = 0.ToString();
                        List<string> matFrameCount = new List<string>();
                        for (int iFrame = 0; iFrame < m_matViewModel.MatColors.Count; iFrame++)
                        {
                            matFrameCount.Add(iFrame.ToString());
                        }
                        m_matViewModel.MatFrame = matFrameCount;
                    }
                }
                else
                {
                    ResizeImage(strImagePath);
                    ConvertImage(strExt);
                }
            }
        }

        private void ConnectCommand(object sender, RoutedEventArgs e)
        {
            if(m_bIsConnected)
            {
                m_serialPort?.Close();
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
        
        protected void RunTask()
        {
            while (true)
            {
                // Update ports
                m_matViewModel.GetPorts();


                // Delay for 50 milliseconds
                Thread.Sleep(50);
            }
        }
        private void SaveCommand(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter writer = File.CreateText(saveFileDialog.FileName))
                {
                    XamlServices.Save(writer, m_matViewModel.MatColors);
                }
            }
        }

        private void SendCommand(object sender, RoutedEventArgs e)
        {
            // Accumulate the bytes
            List<byte> byteList = new List<byte>
            {
                (byte)1,
                (byte)m_matViewModel.MatFrame.Count(), // TODO num of frames
                (byte)32,
                (byte)32
            };
            // Loop through the matrix and convert each hex value to bytes
            foreach(MatrixFrame matFrame in m_matViewModel.MatColors)
            {
                foreach(LedState ledPixel in matFrame.MatPixels)
                {
                    SolidColorBrush rgbValue = ledPixel.Color as SolidColorBrush ?? new SolidColorBrush(Colors.Black);
                    byteList.Add((byte)rgbValue.Color.B);
                    byteList.Add((byte)rgbValue.Color.G);
                    byteList.Add((byte)rgbValue.Color.R);
                    byteList.Add(0);
                }
                string strSpeed = m_matViewModel.AnimationSpeedCurrent.Replace("x", "");
                float.TryParse(strSpeed, out float fSpeed);
                fSpeed = 1 / fSpeed;
                int iSpeed = (int)max(fSpeed * matFrame.iFrameDelay,1);
                byte[] arrSpeed = BitConverter.GetBytes(iSpeed);
                byteList.Add(arrSpeed[0]);
                byteList.Add(arrSpeed[1]);
                byteList.Add(arrSpeed[2]);
                byteList.Add(arrSpeed[3]);
            }
            // Write the accumulated bytes to the serial port
            if(m_bIsConnected && m_matViewModel.Ports.Length > 0)
            {
                m_serialPort?.Write(byteList.ToArray(), 0, byteList.Count);
            }
        }

        private float max(float v1, int v2)
        {
            return v1 > v2 ? v1 : v2;
        }

        private void SendCommand1(object sender, RoutedEventArgs e)
        {
            //open the file using file stream
            FileStream fileStream = new FileStream(@"..\..\Resources\6SstyNn.bin", FileMode.Open, FileAccess.Read, FileShare.Read);

            //store the open file as binary
            BinaryReader binary = new BinaryReader(fileStream, Encoding.GetEncoding(28591));

            //at this point I write to the port
            m_serialPort?.Write(binary.ReadBytes((int)fileStream.Length), 0, (int)fileStream.Length);
        }


        private void ResizeImage(string strImagePath)
        {
            using (var collection = new MagickImageCollection(new FileInfo(strImagePath)))
            {
                collection.Coalesce();
                foreach (var image in collection)
                {
                    if(image.Width > image.Height)
                    {
                        image.Resize(MATRIX_WIDTH, 0);
                    }
                    else
                    {

                        image.Resize(0, MATRIX_HEIGHT); 
                    }
                }
                collection.Write(@"..\..\Resources\current.gif");
            }
        }

        private void ConvertImage(string strExt)
        {
            using (FileStream stream = new FileStream(@"..\..\Resources\current.gif", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                GifBitmapDecoder decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                ObservableCollection<MatrixFrame> matColors = new ObservableCollection<MatrixFrame>();
                List<string> matFrameCount = new List<string> { };

                using (MagickImageCollection collection = new MagickImageCollection(@"..\..\Resources\current.gif"))
                {
                    int j = 0;
                    // Iterate over each frame in the GIF
                    foreach (MagickImage frame in collection)
                    {
                        MatrixFrame matFrame = new MatrixFrame(Colors.Black);
                        matFrameCount.Add(j++.ToString());
                        matFrame.iFrameDelay = frame.AnimationDelay * 10;
                        IPixelCollection<ushort> pixels = frame.GetPixels();
                        IEnumerator<IPixel<ushort>> ePixels = pixels.GetEnumerator();

                        // Calculate the starting position in the 32x32 matrix
                        int startX = (32 - frame.Width) / 2;
                        int startY = (32 - frame.Height) / 2;

                        // Iterate over the pixels of the image and assign RGB values to the matrix
                        for (int row = 0; row < frame.Height; row++)
                        {
                            for (int column = 0; column < frame.Width; column++)
                            {
                                // Get the RGB value of the pixel from the frame
                                ePixels.MoveNext();
                                IMagickColor<ushort> color = ePixels.Current.ToColor();
                                byte r = (byte)color.R;
                                byte g = (byte)color.G;
                                byte b = (byte)color.B;
                                LedState ledState = new LedState(new SolidColorBrush(Color.FromRgb(r, g, b)));

                                // Assign the RGB value to the corresponding position in the matrix
                                int collectionIndex = (startY + row) * 32 + (startX + column);

                                // Assign the RGB value to the corresponding position in the color collection
                                matFrame.MatPixels[collectionIndex] = ledState; // TODO BUG
                            }
                        }
                        matColors.Add(matFrame);
                    }
                    m_matViewModel.MatFrame = matFrameCount;
                    m_matViewModel.MatColors = matColors;
                    m_matViewModel.MatColorsCurrent = m_matViewModel.MatColors[0];
                }
            }
        }

        public void SetDisplay(string strExt)
        {
            DisplaySettings eDisplay = DisplaySettings.NONE;
            switch(strExt)
            {
                case ".jpg":
                case ".png":
                case ".bmp":
                case ".gif":
                    eDisplay = DisplaySettings.GIF; break;
                case ".buf":
                    eDisplay = DisplaySettings.BUF; break;
            }
            m_matViewModel.DisplaySetting = eDisplay;
        }
    }
}
