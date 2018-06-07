using System.Windows;

namespace Fastly_Image_Viewer
{
    public partial class ColorPickerWindow : Window
    {
        public ColorPickerWindow()
        {
            InitializeComponent();

            titleLbl.MouseDown += (s, e) => DragMove();
            closeLbl.Click += (s, e) => Hide();
        }
    }
}
