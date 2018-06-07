using System.Windows;

namespace Fastly_Image_Viewer
{
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();

            versionLbl.Content = $"{Properties.Settings.Default.Version}";

            closeBtn.Click += (s, e) => Hide();
            githubLbl.Click += (s, e) => System.Diagnostics.Process.Start("https://github.com/Rebzzel/Fastly-Image-Viewer");
        }
    }
}