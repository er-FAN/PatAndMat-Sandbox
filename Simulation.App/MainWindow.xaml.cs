using Simulation.App.SimulationObjects;

using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Controls;
using Simulation.Engine.models;
using Simulation.Engine.events;

namespace Simulation.App
{
    public partial class MainWindow : Window
    {
        private readonly List<Ball> balls = new();
        private readonly DispatcherTimer timer = new();
        private readonly Dictionary<Ball, Ellipse> ballVisuals = new();
        SimulationContext simulationContext;
        EventBus eventBus;
        public List<ISimulableObject> iballs { get; set; } = [];
        double width;
        double height;
        public MainWindow()
        {
            width = this.Width;
            height = this.Height;
            eventBus = new EventBus(iballs);
            simulationContext = new SimulationContext(eventBus);
            InitializeComponent();
            InitBalls();
            InitTimer();
        }

        private void InitBalls()
        {
            var random = new Random();

            for (int i = 0; i < 10; i++)
            {
                var ball = new Ball(
                    x: random.Next(100, 700),
                    y: random.Next(100, 500),
                    vx: random.NextDouble() * 4 - 2,
                    vy: random.NextDouble() * 4 - 2,
                    radius: 15
                );


                ball.Behaviors.Add(new MovementBehavior());
                ball.Behaviors.Add(new WallBounceBehavior(width - 10, height - 10));
                ball.Behaviors.Add(new BallCollisionBehavior(balls));
                balls.Add(ball);

                

                SimulationCanvas.Children.Add(ball.shape);
                ballVisuals[ball] = ball.shape;
            }
        }

        private void InitTimer()
        {
            timer.Interval = TimeSpan.FromMilliseconds(16); // حدود 60 FPS
            timer.Tick += (s, e) => UpdateSimulation();
            timer.Start();
        }

        private void UpdateSimulation()
        {
            foreach (var ball in balls)
            {
                foreach (var behavior in ball.Behaviors)
                {
                    behavior.Apply(ball, simulationContext);
                }

                var ellipse = ballVisuals[ball];
                Canvas.SetLeft(ellipse, ball.X - ball.Radius);
                Canvas.SetTop(ellipse, ball.Y - ball.Radius);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // به‌روزرسانی اندازه دیوارها هنگام تغییر اندازه پنجره
            double newWidth = this.Width;
            double newHeight = this.Height;

            foreach (var ball in balls)
            {
                // به‌روزرسانی رفتارها برای هر توپ
                foreach (var behavior in ball.Behaviors.OfType<WallBounceBehavior>())
                {
                    // به‌روزرسانی ابعاد دیوار
                    behavior.UpdateDimensions(newWidth - 10, newHeight - 10);
                }
            }
        }
    }
}
