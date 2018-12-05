using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;

namespace Amporis.WPF.ShapeEditor
{
    public class ShapeEditor : Grid
    {
        #region Fields & props 

        Rectangle rLT, rCT, rRT, rLC, rRC, rLB, rCB, rRB, rCover;
        const double rectSize = 10;

        bool moving = false, resizing = false;
        Point mysPosStart, objPos, objSizeStart;

        public FrameworkElement CapturedElement { get; private set; }

        #endregion

        #region Constructor

        public ShapeEditor()
        {
            Visibility = Visibility.Collapsed;
            rCover = new Rectangle()   // almost transparent cover rectangle for catching clicks
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(rectSize / 2.0),
                Fill = new SolidColorBrush(Color.FromArgb(1, 255, 255, 255)),
            };
            rCover.MouseLeftButtonDown += RCover_MouseLeftButtonDown;
            rCover.MouseLeftButtonUp += RCover_MouseLeftButtonUp;
            rCover.MouseMove += RCover_MouseMove;
            Children.Add(rCover);

            rLT = AddRect("rLT", HorizontalAlignment.Left, VerticalAlignment.Top, Cursors.SizeNWSE);
            rCT = AddRect("rCT", HorizontalAlignment.Center, VerticalAlignment.Top, Cursors.SizeNS);
            rRT = AddRect("rRT", HorizontalAlignment.Right, VerticalAlignment.Top, Cursors.SizeNESW);
            rLC = AddRect("rLC", HorizontalAlignment.Left, VerticalAlignment.Center, Cursors.SizeWE);
            rRC = AddRect("rRC", HorizontalAlignment.Right, VerticalAlignment.Center, Cursors.SizeWE);
            rLB = AddRect("rLB", HorizontalAlignment.Left, VerticalAlignment.Bottom, Cursors.SizeNESW);
            rCB = AddRect("rCB", HorizontalAlignment.Center, VerticalAlignment.Bottom, Cursors.SizeNS);
            rRB = AddRect("rRB", HorizontalAlignment.Right, VerticalAlignment.Bottom, Cursors.SizeNWSE);
        }

        // Method for creating small rectangles at the corners and sides
        private Rectangle AddRect(string name, HorizontalAlignment ha, VerticalAlignment va, Cursor crs)
        {
            var rect = new Rectangle()   // small rectangles at the corners and sides
            {
                HorizontalAlignment = ha,
                VerticalAlignment = va,
                Width = rectSize,
                Height = rectSize,
                Stroke = new SolidColorBrush(Colors.Gray), // small rectangles color
                Fill = new SolidColorBrush(Color.FromArgb(1, 255, 255, 255)),
                Cursor = crs,
                Name = name,
            };
            rect.MouseLeftButtonDown += Rect_MouseLeftButtonDown;
            rect.MouseLeftButtonUp += Rect_MouseLeftButtonUp;
            rect.MouseMove += Rect_MouseMove;
            Children.Add(rect);
            return rect;
        }

        #endregion

        #region Click on the shape - moving

        private void RCover_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            moving = true;
            mysPosStart = e.GetPosition(this.Parent as Canvas);
            objPos = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
            objSizeStart = new Point(rCover.ActualWidth, rCover.ActualHeight);
            rCover.CaptureMouse();
            rCover.Cursor = Cursors.SizeAll;
            e.Handled = true;
        }

        private void RCover_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            moving = false;
            rCover.ReleaseMouseCapture();
            rCover.Cursor = Cursors.Arrow;
        }

        private void RCover_MouseMove(object sender, MouseEventArgs e)
        {
            if (moving)
            {
                var mysPosTed = e.GetPosition(this.Parent as Canvas);
                double x = mysPosTed.X - mysPosStart.X + objPos.X;
                Canvas.SetLeft(this, x);
                double y = mysPosTed.Y - mysPosStart.Y + objPos.Y;
                Canvas.SetTop(this, y);
            }
        }

        #endregion

        #region Click on the conner or side of the shape - resizing

        private void Rect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            resizing = true;
            var rec = sender as FrameworkElement;
            mysPosStart = e.GetPosition(this.Parent as Canvas);
            objPos = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
            objSizeStart = new Point(this.ActualWidth, this.ActualHeight);
            rec.CaptureMouse();
            e.Handled = true;
        }

        private void Rect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            resizing = false;
            var rec = sender as FrameworkElement;
            rec.ReleaseMouseCapture();
        }

        private void Rect_MouseMove(object sender, MouseEventArgs e)
        {
            if (resizing)
            {
                var mysPosTed = e.GetPosition(this.Parent as FrameworkElement);
                var prvek = sender as FrameworkElement;
                if (prvek.Name[1] != 'C')
                {
                    double width;
                    if (prvek.Name[1] == 'L')
                    {
                        width = mysPosStart.X - mysPosTed.X  + objSizeStart.X;
                        Canvas.SetLeft(this, Canvas.GetLeft(this) - (width - Width));
                    } else
                        width = mysPosTed.X - mysPosStart.X + objSizeStart.X;
                    Width = Math.Max(width, 0);
                }
                if (prvek.Name[2] != 'C')
                {
                    double height;
                    if (prvek.Name[2] == 'T')
                    {
                        height = mysPosStart.Y - mysPosTed.Y + objSizeStart.Y;
                        Canvas.SetTop(this, Canvas.GetTop(this) - (height - Height));
                    }
                    else
                        height = mysPosTed.Y - mysPosStart.Y + objSizeStart.Y;
                    Height = Math.Max(height, 0);
                }
            }
        }

        #endregion

        #region Editing shape

        // Start editing a shape (element)
        public void CaptureElement(FrameworkElement element, MouseButtonEventArgs mouse = null)
        {
            if (CapturedElement != null)
            {
                if (CapturedElement == element)
                    return;
                ReleaseElement();
            }
            CapturedElement = element;
            Canvas.SetLeft(this, Canvas.GetLeft(element) - rectSize / 2.0);
            Canvas.SetTop(this, Canvas.GetTop(element) - rectSize / 2.0);
            Width = element.Width + rectSize;
            Height = element.Height + rectSize;
            ((Canvas)element.Parent).Children.Remove(element);
            Children.Insert(0, element);
            element.Margin = new Thickness(rectSize / 2.0);
            element.HorizontalAlignment = HorizontalAlignment.Stretch;
            element.VerticalAlignment = VerticalAlignment.Stretch;
            element.Width = double.NaN;
            element.Height = double.NaN;
            Visibility = Visibility.Visible;
            if (mouse != null)
                RCover_MouseLeftButtonDown(rCover, mouse);
        }

        // End editing shape
        public void ReleaseElement()
        {
            if (CapturedElement == null) return;
            FrameworkElement element = CapturedElement;
            Children.Remove(element);
            ((Canvas)Parent).Children.Add(element);
            Canvas.SetLeft(element, Canvas.GetLeft(this) + rectSize / 2.0);
            Canvas.SetTop(element, Canvas.GetTop(this) + rectSize / 2.0);
            element.Width = Width - rectSize;
            element.Height = Height - rectSize;
            element.Margin = new Thickness(0);
            CapturedElement = null;
            Visibility = Visibility.Collapsed;
        }

        #endregion
    }
}
