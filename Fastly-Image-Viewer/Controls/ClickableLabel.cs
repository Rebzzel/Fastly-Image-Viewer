using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Fastly_Image_Viewer.Controls
{
    class ClickableLabel : Label
    {
        private Brush TempBackground;
        private Brush TempForeground;

        public static readonly DependencyProperty ClickBackgroundProperty =
            DependencyProperty.Register("ClickBackground", typeof(Brush), typeof(Label));

        public Brush ClickBackground
        {
            get => (Brush)GetValue(ClickBackgroundProperty);
            set => SetValue(ClickBackgroundProperty, value);
        }

        public static readonly DependencyProperty ClickForegroundProperty =
            DependencyProperty.Register("ClickForeground", typeof(Brush), typeof(Label), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0, 130, 205))));

        public Brush ClickForeground
        {
            get => (Brush)GetValue(ClickForegroundProperty);
            set => SetValue(ClickForegroundProperty, value);
        }

        public static readonly RoutedEvent ClickEvent = ButtonBase.ClickEvent.AddOwner(typeof(ClickableLabel));

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            CaptureMouse();

            if (IsMouseCaptured)
            {
                TempBackground = Background;
                TempForeground = Foreground;
                Background = ClickBackground;
                Foreground = ClickForeground;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (IsMouseCaptured)
            {
                ReleaseMouseCapture();

                Background = TempBackground;
                Foreground = TempForeground;

                if (IsMouseOver)
                {
                    RaiseEvent(new RoutedEventArgs(ClickEvent, this));
                }

            }
        }
    }
}
