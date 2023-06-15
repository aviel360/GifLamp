﻿using System;
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
using System.Collections;
using PhotoSauce.MagicScaler;
using static GifApp.MatrixViewModel;
using System.Drawing.Imaging;
using System.Windows.Media.Media3D;
using System.Collections.ObjectModel;
using GifApp.Properties;
using ImageMagick;

#nullable enable

namespace GifApp
{
    /// <summary>
    /// Interaction logic for MatrixView.xaml
    /// </summary>
    public partial class MatrixView : UserControl
    {
        const int MATRIX_SIZE = 32;


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

        private void UploadCommand(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.bmp, *.jpg, *.jpeg, *.gif, *.png)|*.bmp;*.jpg;*.jpeg;*.png;*.gif|All files (*.*)|*.*";

            if (dialog.ShowDialog() ?? false)
            {
                ConvertImage(ResizeImage(dialog.FileName));
            }
            try
            {

            }
            catch (Exception)
            {

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
                // Do some work here
                //SendData();
                m_matViewModel.GetPorts();

                // Delay for 50 milliseconds
                Thread.Sleep(50);
            }
        }

        private void SendCommand(object sender, RoutedEventArgs e)
        {
            // Accumulate the bytes
            List<byte> byteList = new List<byte>
            {
                (byte)m_matViewModel.MatFrame.Count(), // TODO num of frames
                (byte)m_matViewModel.DisplaySetting
            };
            // Loop through the matrix and convert each hex value to bytes
           // foreach (m_matColors)
            foreach(MatrixFrame matFrame in m_matViewModel.MatColors)
            {
                foreach(LedState ledPixel in matFrame.MatPixels)
                {
                    SolidColorBrush rgbValue = ledPixel.Color as SolidColorBrush ?? new SolidColorBrush(Colors.Black);
                    byteList.Add((byte)rgbValue.Color.R);
                    byteList.Add((byte)rgbValue.Color.G);
                    byteList.Add((byte)rgbValue.Color.B);
                }
                string strSpeed = m_matViewModel.AnimationSpeedCurrent.Replace("x", "");
                float.TryParse(strSpeed, out float iSpeed);
                byteList.Add((byte)(matFrame.iFrameDelay * iSpeed));
            }
            // Write the accumulated bytes to the serial port
            m_serialPort?.Write(byteList.ToArray(), 0, byteList.Count);
        }
        private void SendData()
        {
            //open the file using file stream
            FileStream fileStream = new FileStream(@"C:\Workspace\GifLamp\GifApp\GifApp\Resources\xw.bin", FileMode.Open, FileAccess.Read, FileShare.Read);

            //store the open file as binary
            BinaryReader binary = new BinaryReader(fileStream, Encoding.GetEncoding(28591));

            //at this point I write to the port
            m_serialPort?.Write(binary.ReadBytes((int)fileStream.Length), 0, (int)fileStream.Length);
        }


        private string ResizeImage(string strImagePath)
        {
            string strExt = System.IO.Path.GetExtension(strImagePath);

            SetDisplay(strExt);
            using (var collection = new MagickImageCollection(new FileInfo(strImagePath)))
            {
                collection.Coalesce();
                foreach (var image in collection)
                {
                    image.Resize(MATRIX_SIZE, 0);
                }
                collection.Write(@"..\..\Resources\current.gif");
            }
            return strExt;
        }

        private void ConvertImage(string strExt)
        {
            using (FileStream stream = new FileStream(@"..\..\Resources\current.gif", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                GifBitmapDecoder decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                ObservableCollection<MatrixFrame> matColors = new ObservableCollection<MatrixFrame>();
                List<string> matFrame = new List<string> { };

                using (MagickImageCollection collection = new MagickImageCollection(@"..\..\Resources\current.gif"))
                {
                    int j = 0;
                    // Iterate over each frame in the GIF
                    foreach (MagickImage frame in collection)
                    {
                        // Iterate over the pixels of the frame
                        int iWidth = (int)frame.Width;
                        int iHeight = (int)frame.Height;
                        int startCol = (MATRIX_SIZE - iWidth) / 2;
                        int startRow = (MATRIX_SIZE - iHeight) / 2;
                        matFrame.Add(j.ToString());
                        MatrixFrame matrixFrame = new MatrixFrame();
                        matrixFrame.iFrameDelay = frame.AnimationDelay;
                        matColors.Add(matrixFrame);
                        IPixelCollection<ushort> pixels = frame.GetPixels();
                        IEnumerator<IPixel<ushort>> ePixels = pixels.GetEnumerator();
                        for (int row = 0; row < MATRIX_SIZE; row++)
                        {
                            for (int col = 0; col < MATRIX_SIZE; col++)
                            {
                                LedState ledState;
                                if (row < startRow || row > iWidth || col < startRow || col > iHeight)
                                {
                                    ledState = new LedState();
                                }
                                else
                                {
                                    ePixels.MoveNext();
                                    IMagickColor<ushort> color = ePixels.Current.ToColor();
                                    byte r = (byte)color.R;
                                    byte g = (byte)color.G;
                                    byte b = (byte)color.B;
                                    ledState = new LedState(new SolidColorBrush(Color.FromRgb(r, g, b)));
                                }
                                matColors[j].MatPixels.Add(ledState);
                            }
                        }
                        j++;
                    }
                    m_matViewModel.MatFrame = matFrame;
                    m_matViewModel.MatColors = matColors;
                }
                m_matViewModel.MatColorsCurrent = m_matViewModel.MatColors[0].MatPixels;
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
                    eDisplay = DisplaySettings.IMAGE; break;
                case ".gif": 
                    eDisplay = DisplaySettings.GIF; break;
            }
            m_matViewModel.DisplaySetting = eDisplay;
        }
    }
}
