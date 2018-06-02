using System;
using System.Collections.Generic;
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
        ColorPickerWindow PickerWindow;
        InfoWindow InfoWindow;

        public MainWindow()
        {
            InitializeComponent();

            PickerWindow = new ColorPickerWindow();
            InfoWindow = new InfoWindow();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PickerWindow.Close();
            InfoWindow.Close();
        }

        private void openBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void colorPickerBtn_Click(object sender, RoutedEventArgs e)
        {
            PickerWindow.Show();
        }

        private void zoomInBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void zoomReloadBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void zoomOutBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void infoBtn_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow.ShowDialog();
        }
    }
}
