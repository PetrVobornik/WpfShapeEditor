using System.Windows;
using System.Windows.Input;

namespace Amporis.WPF.ShapeEditor.Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShapeEditorControl.CaptureElement(sender as FrameworkElement, e);
            e.Handled = true;
        }

        private void MainArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShapeEditorControl.ReleaseElement();
        }
    }
}
