using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Fastly_Image_Viewer
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            if (Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", "FastlyImageViewer", null) != null)
                autoRunCheckBox.IsChecked = true;

            switch (Properties.Settings.Default.CloseType)
            {
                case 0:
                    radio1.IsChecked = true;
                    break;
                case 1:
                    radio2.IsChecked = true;
                    break;
            }
        }

        private void titleLbl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void autoRunCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");

            if (autoRunCheckBox.IsChecked == true)
                key.SetValue("FastlyImageViewer", Assembly.GetEntryAssembly().Location);
            else
                key.DeleteValue("FastlyImageViewer");

            key.Close();
        }

        private void radio1_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.CloseType = 0;
        }

        private void radio2_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.CloseType = 1;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void applyBtn_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            this.Hide();
        }
    }
}
