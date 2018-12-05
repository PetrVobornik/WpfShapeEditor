# ShapeEditor for WPF
Shape editor for WPF can be used when you want to allow to the user to work with shapes or images (moving and resizing) or to define his own custom arrange of the workspace.

<p align="center">
<img src="https://github.com/PetrVobornik/WpfShapeEditor/raw/master/images/shape-editor-demo.gif" alt="Shape Editor Demo" />
</p>


## Usage

Add file [ShapeEditor.cs](https://github.com/PetrVobornik/WpfShapeEditor/blob/master/ShapeEditor.cs) to your project. Shape or Image (or any other compatible element) must be placed on the Canvas container. At the same Canvas you must place one instance of ShapeEditor control. Add to each element a response to an event MouseLeftButtonDown by calling ShapeElement.CaptureElement method. That’s all.


**XAML**
```xaml
<Window x:Class="Amporis.WPF.ShapeEditor.Demo.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:se="clr-namespace:Amporis.WPF.ShapeEditor"
        mc:Ignorable="d"
        Title="Shape Editor Demo" Height="320" Width="480">
    <Canvas Name="MainArea" MouseLeftButtonDown="MainArea_MouseLeftButtonDown" Background="DarkGray">
        <Ellipse Fill="Yellow" Stroke="Black" Width="100" Height="100" Canvas.Left="67" Canvas.Top="87" 
                 MouseLeftButtonDown="Shape_MouseLeftButtonDown" />
        <Rectangle Fill="Lime" Stroke="Black" Width="100" Height="100" Canvas.Left="241" Canvas.Top="38" 
                   MouseLeftButtonDown="Shape_MouseLeftButtonDown" />
        <Image Height="69" Width="114" Canvas.Left="269" Canvas.Top="182" Source="czech-flag.png" Stretch="Fill"
               MouseLeftButtonDown="Shape_MouseLeftButtonDown" />
        <se:ShapeEditor x:Name="ShapeEditorControl" Height="100" Canvas.Left="46" Canvas.Top="170" Width="100"/>
    </Canvas>
</Window>
```


**C#**
```csharp
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
```
