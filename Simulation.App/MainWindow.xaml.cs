using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using Simulation.Engine.models;
using Simulation.Engine.Components.Render;
using Simulation.App.Models;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Simulation.Engine.events;

namespace Simulation.App
{
    public partial class MainWindow : Window
    {
        private readonly List<IRenderable> _objects = new();
        private readonly IContext mainContext;
        private readonly DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            mainContext = new SimulationContext();

            SnakeBodyContext snake = new SnakeBodyContext(startX: 5, startY: 5, initialLength: 4, color: System.Drawing.Color.Blue);
            mainContext.ChildContexts.Add(snake);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(500); // حدود 60 فریم بر ثانیه
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Key_Down(object sender, KeyEventArgs e)
        {
            string input = "user_input.any";
            if (e.Key == Key.A)
            {
                if (mainContext.ChildContexts[0] is SnakeBodyContext snake)
                {
                    input = "user_input.A";
                }
            }
            if (e.Key == Key.D)
            {
                if (mainContext.ChildContexts[0] is SnakeBodyContext snake)
                {
                    input = "user_input.D";
                }
            }
            if (e.Key == Key.S)
            {
                if (mainContext.ChildContexts[0] is SnakeBodyContext snake)
                {
                    input = "user_input.S";
                }
            }
            if (e.Key == Key.W)
            {
                if (mainContext.ChildContexts[0] is SnakeBodyContext snake)
                {
                    input = "user_input.W";
                }
            }
            UserInputEvent inputevent = new UserInputEvent();
            inputevent.Type = input;
            mainContext.ContextEventBus.Publish(inputevent);
        }

        public class UserInputEvent : ISimulationEvent
        {
            public string Type { get; set; }
            public ISimulableObject Source { get; set; }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            mainContext.Update();

            // پاک کردن رسم قبلی
            RenderCanvas.Children.Clear();

            List<WpfRender> renderables = mainContext.Objects
                .Select(obj => obj.GetComponent<WpfRender>())
                .Where(r => r != null)
                .Cast<WpfRender>()
                .ToList();

            foreach (var childContext in mainContext.ChildContexts)
            {
                renderables.AddRange(childContext.Objects
                .Select(obj => obj.GetComponent<WpfRender>())
                .Where(r => r != null)
                .Cast<WpfRender>()
                .ToList());
            }

            foreach (IRenderable obj in renderables)
            {
                if (obj.SpriteId != null)
                {
                    Image image = new Image
                    {
                        Width = obj.Size.X,
                        Height = obj.Size.Y,
                        Source = new BitmapImage(new Uri(obj.SpriteId, UriKind.RelativeOrAbsolute))
                    };
                    Canvas.SetLeft(image, obj.Position.X);
                    Canvas.SetTop(image, obj.Position.Y);
                    RenderCanvas.Children.Add(image);
                }
                else
                {
                    var rect = new Rectangle
                    {
                        Width = obj.Size.X,
                        Height = obj.Size.Y,
                        Fill = new SolidColorBrush(Color.FromRgb(obj.Color.R, obj.Color.G, obj.Color.B))
                    };
                    Canvas.SetLeft(rect, obj.Position.X);
                    Canvas.SetTop(rect, obj.Position.Y);
                    RenderCanvas.Children.Add(rect);
                }
            }
        }
    }

}
