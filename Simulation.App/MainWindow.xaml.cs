

using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Controls;
using Simulation.Engine.models;
using Simulation.Engine.events;
using Simulation.App.Models;
using System.Diagnostics;

namespace Simulation.App
{
    public partial class MainWindow : Window
    {
        private Graph _graph;
        SimulationContext _context;
        private DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            _context = new SimulationContext();
            _graph = new Graph();
            CreateStreet();
            CreateCar();
            DrawSimulation();
            InitializeTimer();
        }
        public List<Street> streets { get; set; } = [];
        public List<Car> cars { get; set; } = [];

        private void InitializeTimer()
        {
            MoveBehavior moveBehavior = new MoveBehavior();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30); // فریم‌ها با هر 30 میلی‌ثانیه به‌روزرسانی می‌شوند
            timer.Tick += (sender, e) =>
            {
                // حرکت ماشین‌ها
                foreach (var car in cars)
                {
                    moveBehavior.Apply(car, _context);
                }

                // به‌روزرسانی رسم شبیه‌سازی
                SimulationCanvas.Children.Clear();
                DrawSimulation();
            };
            timer.Start();
        }


        public void CreateStreet()
        {
            // ایجاد خیابان‌ها
            var street1 = new Street("Street 1", 60);
            street1.Start = new Node { X = 0, Y = 0 };
            street1.End = new Node { X = 100, Y = 100 };
            streets.Add(street1);
            var street2 = new Street("Street 2", 50);
            street2.Start = new Node { X = 100, Y = 100 };
            street2.End = new Node { X = 200, Y = 500 };
            streets.Add(street2);
            var street3 = new Street("Street 3", 50);
            street3.Start = new Node { X = 100, Y = 100 };
            street3.End = new Node { X = 400, Y = 100 };
            streets.Add(street3);

            var street4 = new Street("Street 4", 50);
            street4.Start = new Node { X = 400, Y = 100 };
            street4.End = new Node { X = 400, Y = 400 };
            streets.Add(street4);

            var street5 = new Street("Street 5", 50)
            {
                Start = new Node { X = 400, Y = 400 },
                End = new Node { X = 200, Y = 500 }
            };
            streets.Add(street5);

            var street6 = new Street("Street 6", 50)
            {
                Start = new Node { X = 400, Y = 100 },
                End = new Node { X = 600, Y = 300 }
            };
            streets.Add(street6);

            var street7 = new Street("Street 7", 50)
            {
                Start = new Node { X = 200, Y = 500 },
                End = new Node { X = 600, Y = 500 }
            };
            streets.Add(street7);



            _graph.AddStreet(street1);
            _graph.AddStreet(street2);
            _graph.AddStreet(street3);
            _graph.AddStreet(street4);
            _graph.AddStreet(street5);
            _graph.AddStreet(street6);
            _graph.AddStreet(street7);
        }

        public void CreateCar()
        {
            var car = new Car(new EventBus(), _graph);
            car.Location = new Node { X = 0, Y = 0 };
            car.Goal = new Node { X = 600, Y = 300 };
            car.CalculateRoute();
            car.Route.RemoveAt(0);
            car.Route.Add(car.Goal);
            car.Speed = 5;
            cars.Add(car);

            
            // مسیر پیدا شده
            foreach (var node in car.Route)
            {
                Debug.WriteLine($"Car should travel through {node.X},{node.Y}");
            }
        }

        private void DrawSimulation()
        {
            // رسم خیابان‌ها
            foreach (var street in streets)
            {
                DrawStreet(street);
            }

            // رسم ماشین‌ها
            foreach (var car in cars)
            {
                DrawCar(car);
            }
        }

        // رسم یک خیابان
        private void DrawStreet(Street street)
        {
            Line line = new Line
            {
                X1 = street.Start.X,
                Y1 = street.Start.Y,
                X2 = street.End.X,
                Y2 = street.End.Y,
                Stroke = Brushes.Black,
                StrokeThickness = 10
            };

            SimulationCanvas.Children.Add(line);
        }

        // رسم یک ماشین
        private void DrawCar(Car car)
        {
            Ellipse carShape = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Blue
            };

            // موقعیت ماشین
            Canvas.SetLeft(carShape, car.X);
            Canvas.SetTop(carShape, car.Y);

            SimulationCanvas.Children.Add(carShape);
        }
    }
}
