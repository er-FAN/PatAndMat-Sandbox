using Simulation.Engine.Components.Render;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Simulation.App
{
    public class WpfRenderer
    {
        private readonly Canvas _canvas;
        private readonly List<IRenderable> _objects;

        public WpfRenderer(Canvas canvas, List<IRenderable> objects)
        {
            _canvas = canvas;
            _objects = objects;
            CompositionTarget.Rendering += OnRendering;
        }

        private void OnRendering(object? sender, EventArgs e)
        {
            _canvas.Children.Clear();

            foreach (IRenderable obj in _objects)
            {
                if (obj.SpriteId != null)
                {
                    Image image = new Image
                    {
                        //Source = obj.Texture,
                        Width = obj.Size.X,
                        Height = obj.Size.Y
                    };
                    Canvas.SetLeft(image, obj.Position.X);
                    Canvas.SetTop(image, obj.Position.Y);
                    _canvas.Children.Add(image);
                }
                else
                {
                    var rect = new Rectangle
                    {
                        Width = obj.Size.X,
                        Height = obj.Size.Y,
                        Fill = new SolidColorBrush(Colors.Blue)
                    };
                    Canvas.SetLeft(rect, obj.Position.X);
                    Canvas.SetTop(rect, obj.Position.Y);
                    _canvas.Children.Add(rect);
                }
            }
        }

        public void Stop()
        {
            CompositionTarget.Rendering -= OnRendering;
        }
    }

}
