using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MainWPF
{
    /// <summary>
    /// Логика взаимодействия для MyModalWindow.xaml
    /// </summary>
    public partial class MyModalWindow : Window
    {
        public MyModalWindow()
        {
            InitializeComponent();
        }

        Point moveStart;

        private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                !(e.OriginalSource is ButtonBase || e.OriginalSource is Thumb || e.OriginalSource is TextBoxBase))
            {
                var deltaPos = e.GetPosition(this) - moveStart;
                Left += deltaPos.X;
                Top += deltaPos.Y;
            }
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                moveStart = e.GetPosition(this);
            }
        }
    }
}
