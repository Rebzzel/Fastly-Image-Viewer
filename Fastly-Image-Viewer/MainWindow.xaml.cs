using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Fastly_Image_Viewer
{
    public partial class MainWindow : Window
    {
        public static string[] Args = Environment.GetCommandLineArgs();

        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private Core.Image _image;
        private ColorPickerWindow _pickerWindow;
        private SettingsWindow _settingsWindow;
        private InfoWindow _infoWindow;

        public MainWindow()
        {
            InitializeComponent();

            var context = new System.Windows.Forms.ContextMenu();
            context.MenuItems.Add("Quit", (s, e) => Environment.Exit(0));

            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.Icon = Properties.Resources.FIV;
            _notifyIcon.Text = "Fastly Image Viewer";
            _notifyIcon.ContextMenu = context;
            _notifyIcon.Click += notifyIcon_Click;
            _notifyIcon.Visible = true;

            _pickerWindow = new ColorPickerWindow();
            _settingsWindow = new SettingsWindow();
            _infoWindow = new InfoWindow();

            colorPickerBtn.Click += (s, e) => _pickerWindow.Show();
            settingsBtn.Click += (s, e) => _settingsWindow.Show();
            infoBtn.Click += (s, e) => _infoWindow.Show();

            if (Args.Length > 1)
                OpenImage(Args[1]);
        }

        public void OpenImage(string path)
        {
            _image = new Core.Image(path);

            var width = _image.Bitmap.Width;
            var height = _image.Bitmap.Height;

            var fileInfo = new FileInfo(path);

            infoLbl.Visibility = Visibility.Visible;
            infoLbl2.Content = $"{fileInfo.Name}\n{String.Format("{0:f2}", (double)fileInfo.Length / 1024)} KB\n{width} x {height}";

            while (width > SystemParameters.PrimaryScreenWidth * 70 / 100)
            {
                width = width * 70 / 100;
            }

            while (height > SystemParameters.PrimaryScreenHeight * 70 / 100)
            {
                height = height * 70 / 100;
            }

            image.Width = width;
            image.Height = height;
            grid.Visibility = Visibility.Hidden;
            image.Source = _image.GetBitmapSource();

            saveAsBtn.IsEnabled = true;
            zoomInBtn.IsEnabled = true;
            zoomReloadBtn.IsEnabled = true;
            zoomOutBtn.IsEnabled = true;
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            ShowInTaskbar = true;

            Show();
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                OpenImage(files[0]);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShowInTaskbar = false;

            _pickerWindow.Close();
            _settingsWindow.Close();
            _infoWindow.Close();
        }

        private void Window_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (_image == null)
                return;

            var width = _image.Bitmap.Width;
            var height = _image.Bitmap.Height;

            while (width > SystemParameters.PrimaryScreenWidth * 70 / 100)
            {
                width = width * 70 / 100;
            }

            while (height > SystemParameters.PrimaryScreenHeight * 70 / 100)
            {
                height = height * 70 / 100;
            }

            if (e.Delta > 0)
            {
                if (image.Width > (SystemParameters.PrimaryScreenWidth * 70 / 100) || image.Height > (SystemParameters.PrimaryScreenHeight * 70 / 100))
                    return;

                image.Width += width * 5 / 100;
                image.Height += height * 5 / 100;
            } else
            {
                if (image.Width - width * 5 / 100 <= 0 || image.Height - height * 5 / 10 <= 0)
                    return;

                image.Width -= width * 5 / 100;
                image.Height -= height * 5 / 100;
            }
        }

        private void openBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter =
                "All supported (*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.ico;*.tiff;*.wmf)|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.ico;*.tiff;*.wmf|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png|" +
                "Graphics Interchange Format (*.gif)|*.gif|" +
                "Icon (*.ico)|*.ico|" +
                "Other (*.tiff;*.wmf)|*.tiff;*.wmf|" +
                "All files (*.*)|*.*";

            if (dialog.ShowDialog() is true)
                OpenImage(dialog.FileName);
        }

        private void saveAsBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter =
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png|" +
                "Graphics Interchange Format (*.gif)|*.gif|" +
                "Icon (*.ico)|*.ico";

            if (dialog.ShowDialog() is true)
            {
                switch (dialog.FilterIndex)
                {
                    case 1:
                        _image.Bitmap.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case 2:
                        _image.Bitmap.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 3:
                        _image.Bitmap.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case 4:
                        _image.Bitmap.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Icon);
                        break;
                }
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            _pickerWindow.Hide();
            _settingsWindow.Hide();
            _infoWindow.Hide();
        }

        private void zoomInBtn_Click(object sender, RoutedEventArgs e)
        {
            var width = _image.Bitmap.Width;
            var height = _image.Bitmap.Height;

            while (width > SystemParameters.PrimaryScreenWidth * 70 / 100)
            {
                width = width * 70 / 100;
            }

            while (height > SystemParameters.PrimaryScreenHeight * 70 / 100)
            {
                height = height * 70 / 100;
            }

            if (image.Width > (SystemParameters.PrimaryScreenWidth * 70 / 100) || image.Height > (SystemParameters.PrimaryScreenHeight * 70 / 100))
                return;

            image.Width += width * 5 / 100;
            image.Height += height * 5 / 100;
        }

        private void zoomReloadBtn_Click(object sender, RoutedEventArgs e)
        {
            var width = _image.Bitmap.Width;
            var height = _image.Bitmap.Height;

            while (width > SystemParameters.PrimaryScreenWidth * 70 / 100)
            {
                width = width * 70 / 100;
            }

            while (height > SystemParameters.PrimaryScreenHeight * 70 / 100)
            {
                height = height * 70 / 100;
            }

            image.Width = width;
            image.Height = height;
        }

        private void zoomOutBtn_Click(object sender, RoutedEventArgs e)
        {
            var width = _image.Bitmap.Width;
            var height = _image.Bitmap.Height;

            while (width > SystemParameters.PrimaryScreenWidth * 70 / 100)
            {
                width = width * 70 / 100;
            }

            while (height > SystemParameters.PrimaryScreenHeight * 70 / 100)
            {
                height = height * 70 / 100;
            }

            if (image.Width - width * 5 / 100 <= 0 || image.Height - height * 5 / 10 <= 0)
                return;

            image.Width -= width * 5 / 100;
            image.Height -= height * 5 / 100;
        }
    }
}