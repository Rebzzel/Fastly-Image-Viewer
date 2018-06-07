using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Fastly_Image_Viewer.Core
{
    public class Image
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public Bitmap Bitmap { get; }

        public Image(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            Bitmap = new Bitmap(path);
        }

        public BitmapSource GetBitmapSource()
        {
            IntPtr bitmapHandle = Bitmap.GetHbitmap();
            BitmapSource source = Imaging.CreateBitmapSourceFromHBitmap(bitmapHandle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(bitmapHandle);

            return source;
        }
    }
}
