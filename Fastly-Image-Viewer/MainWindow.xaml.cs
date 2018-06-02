﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace Fastly_Image_Viewer
{
    public partial class MainWindow : Window
    {
        Bitmap Bitmap;

        ColorPickerWindow PickerWindow;
        InfoWindow InfoWindow;

        public MainWindow()
        {
            InitializeComponent();

            this.PickerWindow = new ColorPickerWindow();
            this.InfoWindow = new InfoWindow();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.PickerWindow.Close();
            this.InfoWindow.Close();
        }

        private void openBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog().Value)
            {
                this.grid.Visibility = Visibility.Hidden;

                this.Bitmap = new Bitmap(dialog.FileName);
                MemoryStream stream = new MemoryStream();
                this.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                this.image.Width = bitmapImage.Width;
                this.image.Height = bitmapImage.Height;
                this.image.Source = bitmapImage;

                this.infoLbl.Visibility = Visibility.Visible;
                this.infoLbl2.Content = $"{dialog.SafeFileName}\n{String.Format("{0:f2}", (double)new FileInfo(dialog.FileName).Length / 1024)} KB\n{Bitmap.Width} x {Bitmap.Height}";

                this.saveAsBtn.IsEnabled = true;
                this.zoomInBtn.IsEnabled = true;
                this.zoomReloadBtn.IsEnabled = true;
                this.zoomOutBtn.IsEnabled = true;
            }  
        }

        private void saveAsBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            if (dialog.ShowDialog().Value)
            {
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: hide window to tray
        }

        private void colorPickerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.PickerWindow.Show();
        }

        private void zoomInBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: zoom in algorithm
        }

        private void zoomReloadBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: zoom reload algorithm
        }

        private void zoomOutBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: zoom out algorithm
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: open settings window
        }

        private void infoBtn_Click(object sender, RoutedEventArgs e)
        {
            this.InfoWindow.ShowDialog();
        }
    }
}
