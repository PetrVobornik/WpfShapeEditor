# ShapeEditor for WPF
Shape editor for WPF can be used when you want to allow to the user to work with shapes or images (moving and resizing) or to define his own custom arrange of the workspace.

<p align="center">
<img src="https://github.com/PetrVobornik/WpfShapeEditor/raw/master/images/shape-editor-demo.gif" alt="Shape Editor Demo" />
</p>


## Usage

Add file [ShapeEditor.cs](https://github.com/PetrVobornik/WpfShapeEditor/blob/master/ShapeEditor.cs) to your project. Shape or Image (or any other compatible element) must be placed on the Canvas container. At the same Canvas you must place one instance of ShapeEditor control. Add to each element a response to an event MouseLeftButtonDown by calling ShapeElement.CaptureElement method. That’s all.
