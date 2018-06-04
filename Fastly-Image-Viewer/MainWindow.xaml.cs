using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
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
        BitmapImage Image;

        System.Windows.Forms.NotifyIcon TrayIcon;

        ColorPickerWindow PickerWindow;
        SettingsWindow SettingsWindow;
        InfoWindow InfoWindow;

        public MainWindow()
        {
            InitializeComponent();

            if (Properties.Settings.Default.IsFirstRun)
            {
                if (MessageBox.Show("Add to autorun?", "Fastly Image Viewer | First Run", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
                    key.SetValue("FastlyImageViewer", Assembly.GetEntryAssembly().Location);
                    key.Close();
                }

                Properties.Settings.Default.IsFirstRun = false;
                Properties.Settings.Default.Save();
            }

            this.ShowInTaskbar = false;

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
                this.OpenImage(args[1]);

            this.TrayIcon = new System.Windows.Forms.NotifyIcon();
            this.TrayIcon.Icon = Properties.Resources.FIV;
            this.TrayIcon.Text = "Fastly Image Viewer";
            this.TrayIcon.Click += TrayIcon_Click;

            this.PickerWindow = new ColorPickerWindow();
            this.SettingsWindow = new SettingsWindow();
            this.InfoWindow = new InfoWindow();
        }

        public void OpenImage(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            this.grid.Visibility = Visibility.Hidden;

            this.Bitmap = new Bitmap(path);
            MemoryStream stream = new MemoryStream();
            this.Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            stream.Position = 0;

            this.Image = new BitmapImage();
            this.Image.BeginInit();
            this.Image.StreamSource = stream;
            this.Image.CacheOption = BitmapCacheOption.OnLoad;
            this.Image.EndInit();

            double width = this.Image.Width;
            double height = this.Image.Height;

            while (width > SystemParameters.PrimaryScreenWidth * 80 / 100)
            {
                width = width * 80 / 100;
            }

            while (height > SystemParameters.PrimaryScreenHeight * 80 / 100)
            {
                height = height * 80 / 100;
            }

            this.image.Width = width;
            this.image.Height = height;
            this.image.Source = this.Image;

            FileInfo fileInfo = new FileInfo(path);

            this.infoLbl.Visibility = Visibility.Visible;
            this.infoLbl2.Content = $"{fileInfo.Name}\n{String.Format("{0:f2}", (double)fileInfo.Length / 1024)} KB\n{Bitmap.Width} x {Bitmap.Height}";

            this.saveAsBtn.IsEnabled = true;
            this.zoomInBtn.IsEnabled = true;
            this.zoomReloadBtn.IsEnabled = true;
            this.zoomOutBtn.IsEnabled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.PickerWindow.Close();
            this.SettingsWindow.Close();
            this.InfoWindow.Close();
        }

        private void TrayIcon_Click(object sender, EventArgs e)
        {
            this.TrayIcon.Visible = false;

            this.Show();
        }

        private void openBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = 
                "All supported (*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.ico;*.tiff;*.wmf)|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.ico;*.tiff;*.wmf|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png|" +
                "Graphics Interchange Format (*.gif)|*.gif|" +
                "Icon (*.ico)|*.ico|" +
                "Other (*.tiff;*.wmf)|*.tiff;*.wmf|" +
                "All files (*.*)|*.*";

            if (dialog.ShowDialog().Value)
                OpenImage(dialog.FileName);
        }

        private void saveAsBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter =
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png|" +
                "Graphics Interchange Format (*.gif)|*.gif|" +
                "Icon (*.ico)|*.ico";

            if (dialog.ShowDialog().Value)
            {
                switch (dialog.FilterIndex)
                {
                    case 1:
                        this.Bitmap.Save(dialog.FileName, ImageFormat.Jpeg);
                        break;
                    case 2:
                        this.Bitmap.Save(dialog.FileName, ImageFormat.Png);
                        break;
                    case 3:
                        this.Bitmap.Save(dialog.FileName, ImageFormat.Gif);
                        break;
                    case 4:
                        this.Bitmap.Save(dialog.FileName, ImageFormat.Icon);
                        break;
                }
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.TrayIcon.Visible = true;

            if (this.PickerWindow.IsActive)
                this.PickerWindow.Hide();

            if (this.SettingsWindow.IsActive)
                this.SettingsWindow.Hide();

            if (this.SettingsWindow.IsActive)
                this.InfoWindow.Hide();

            switch (Properties.Settings.Default.CloseType)
            {
                case 0:
                    this.Hide();
                    break;
                case 1:
                    Environment.Exit(0);
                    break;
            }
            
        }

        private void colorPickerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.PickerWindow.Show();
        }

        private void zoomInBtn_Click(object sender, RoutedEventArgs e)
        {
            double width = Bitmap.Width;
            double height = Bitmap.Height;

            while (width > SystemParameters.PrimaryScreenWidth * 80 / 100)
            {
                width = width * 80 / 100;
            }

            while (height > SystemParameters.PrimaryScreenHeight * 80 / 100)
            {
                height = height * 80 / 100;
            }

            if (image.Width > (SystemParameters.PrimaryScreenWidth * 80 / 100) || image.Height > (SystemParameters.PrimaryScreenHeight * 80 / 100))
                return;

            image.Width += width * 5 / 100;
            image.Height += height * 5 / 100;
        }

        private void zoomReloadBtn_Click(object sender, RoutedEventArgs e)
        {
            double width = this.Image.Width;
            double height = this.Image.Height;

            while (width > SystemParameters.PrimaryScreenWidth * 80 / 100)
            {
                width = width * 80 / 100;
            }

            while (height > SystemParameters.PrimaryScreenHeight * 80 / 100)
            {
                height = height * 80 / 100;
            }

            image.Width = width;
            image.Height = height;
        }

        private void zoomOutBtn_Click(object sender, RoutedEventArgs e)
        {
            double width = Bitmap.Width;
            double height = Bitmap.Height;

            while (width > SystemParameters.PrimaryScreenWidth * 80 / 100)
            {
                width = width * 80 / 100;
            }

            while (height > SystemParameters.PrimaryScreenHeight * 80 / 100)
            {
                height = height * 80 / 100;
            }

            if (image.Width - width * 5 / 100 <= 0 || image.Height - height * 5 / 10 <= 0)
                return;

            image.Width -= width * 5 / 100;
            image.Height -= height * 5 / 100;
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            this.SettingsWindow.Show();
        }

        private void infoBtn_Click(object sender, RoutedEventArgs e)
        {
            this.InfoWindow.ShowDialog();
        }
    }
}
