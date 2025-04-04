using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using Simulation.Engine.models;
using Simulation.Engine.tasks;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace SimulationApp
{
    public partial class MainWindow : Window
    {
        private List<PhysicalObject> objects = new();
        private bool isRunning = false;
        public object SelectedObject { get; set; }
        private Point lastMousePosition;  // ذخیره آخرین موقعیت ماوس
        private bool isDragging = false;  // وضعیت درگ

        private double zoomFactor = 1.0;
        private const double zoomStep = 0.1;
        private const double minZoom = 0.5;
        private const double maxZoom = 6.0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSimulation();
            CreateGridBackground();
        }

        private void ScrollViewer_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            lastMousePosition = e.GetPosition(MapScrollViewer);
            isDragging = true;
            MapScrollViewer.CaptureMouse();
        }

        private void ScrollViewer_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            MapScrollViewer.ReleaseMouseCapture();
        }

        // 📌 درگ کردن با کلیک راست
        private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newMousePosition = e.GetPosition(MapScrollViewer);
                double offsetX = lastMousePosition.X - newMousePosition.X;
                double offsetY = lastMousePosition.Y - newMousePosition.Y;

                MapScrollViewer.ScrollToHorizontalOffset(MapScrollViewer.HorizontalOffset + offsetX);
                MapScrollViewer.ScrollToVerticalOffset(MapScrollViewer.VerticalOffset + offsetY);

                lastMousePosition = newMousePosition;
            }
        }

        // 🎨 رویداد کلیک روی Canvas برای انتخاب شیء
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(SimulationCanvas);

            foreach (var child in SimulationCanvas.Children)
            {
                if (child is Shape shape && shape.IsMouseOver)
                {
                    SelectedObject = shape.Tag; // ذخیره شیء انتخاب‌شده
                    PropertyGridControl.Content = SelectedObject; // نمایش در PropertyGrid
                    return;
                }
            }
        }

        // 🎮 دکمه شروع شبیه‌سازی
        private void StartSimulation_Click(object sender, RoutedEventArgs e)
        {
            if (!isRunning)
                StartSimulation();
        }

        // ⏸️ دکمه توقف شبیه‌سازی
        private void StopSimulation_Click(object sender, RoutedEventArgs e)
        {
            isRunning = false;
        }

        // 🔄 دکمه ریست
        private void ResetSimulation_Click(object sender, RoutedEventArgs e)
        {
            isRunning = false;
            world = new World("زمین", 2000, 2000, 100, 100);
            InitializeSimulation();
            SelectedObject = null; // ریست کردن PropertyGrid
        }


        private void CreateGridBackground()
        {
            int gridSize = 50; // اندازه هر واحد
            DrawingVisual gridVisual = new DrawingVisual();
            using (DrawingContext dc = gridVisual.RenderOpen())
            {
                Pen gridPen = new Pen(Brushes.Gray, 0.5);

                // رسم خطوط عمودی
                for (int x = 0; x < 2000; x += gridSize)
                {
                    dc.DrawLine(gridPen, new Point(x, 0), new Point(x, 2000));
                }

                // رسم خطوط افقی
                for (int y = 0; y < 2000; y += gridSize)
                {
                    dc.DrawLine(gridPen, new Point(0, y), new Point(2000, y));
                }
            }

            // افزودن شبکه به پس‌زمینه‌ی Canvas
            VisualBrush gridBrush = new VisualBrush(gridVisual);
            SimulationCanvas.Background = gridBrush; // تنظیم پس‌زمینه‌ی Canvas به شبکه
        }
        World world = new World("زمین", 2000, 2000, 100, 100);
        private void InitializeSimulation()
        {
            objects.Clear();
            SimulationCanvas.Children.Clear();
            SimulationCanvas.Width = world.Width;
            SimulationCanvas.Height = world.Height;
            var human1 = new LivingBeing("آدم", new Location(500, 600));

            human1.Tasks.Add(new SearchTask(human1,new EdibleObject()));
            human1.Width = 1;
            human1.Height = 1;
            world.AddEntity(human1);
            objects.AddRange(world.EdibleObjects);
            objects.Add(human1);

            DrawObjects();
        }

        private void DrawObjects()
        {
            SimulationCanvas.Children.Clear();

            foreach (var lake in world.Lakes)
            {
                if (lake.Points.Count < 4) continue; // نیاز به حداقل 4 نقطه برای Catmull-Rom

                PathFigure pathFigure = new PathFigure
                {
                    StartPoint = new System.Windows.Point(lake.Points[0].X, lake.Points[0].Y)
                };

                // استفاده از Catmull-Rom Spline برای ایجاد منحنی نرم
                for (int i = 1; i < lake.Points.Count - 2; i++)
                {
                    var p0 = lake.Points[i - 1];
                    var p1 = lake.Points[i];
                    var p2 = lake.Points[i + 1];
                    var p3 = lake.Points[i + 2];

                    // تقسیم منحنی به بخش‌های کوچک‌تر برای همواری بیشتر
                    for (float t = 0; t <= 1; t += 0.1f)
                    {
                        float x = 0.5f * (
                            (2 * p1.X) +
                            (-p0.X + p2.X) * t +
                            (2 * p0.X - 5 * p1.X + 4 * p2.X - p3.X) * t * t +
                            (-p0.X + 3 * p1.X - 3 * p2.X + p3.X) * t * t * t
                        );

                        float y = 0.5f * (
                            (2 * p1.Y) +
                            (-p0.Y + p2.Y) * t +
                            (2 * p0.Y - 5 * p1.Y + 4 * p2.Y - p3.Y) * t * t +
                            (-p0.Y + 3 * p1.Y - 3 * p2.Y + p3.Y) * t * t * t
                        );

                        pathFigure.Segments.Add(new LineSegment(new System.Windows.Point(x, y), true));
                    }
                }

                // بستن شکل
                pathFigure.IsClosed = true;

                // ایجاد PathGeometry
                PathGeometry pathGeometry = new PathGeometry();
                pathGeometry.Figures.Add(pathFigure);

                // رسم Path
                Path lakeShape = new Path
                {
                    Data = pathGeometry,
                    Fill = Brushes.Blue,
                    Stroke = Brushes.DarkBlue,
                    StrokeThickness = 2
                };

                // اضافه کردن به Canvas
                SimulationCanvas.Children.Add(lakeShape);
            }

            foreach (var obj in world.EdibleObjects)
            {
                // ایجاد کنترل Image
                Image image = new()
                {
                    Width = Math.Max(obj.Width * 10, 5),
                    Height = Math.Max(obj.Height * 10, 5),
                    Source = new BitmapImage(new Uri("C:\\Users\\Erfan\\source\\Simulation\\Simulation.App\\Images\\tree.png")),
                    Tag = obj
                };

                Canvas.SetLeft(image, obj.Location.X);
                Canvas.SetTop(image, obj.Location.Y);

                image.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
                SimulationCanvas.Children.Add(image);
            }

            foreach (var obj in world.Entities)
            {
                // ایجاد کنترل Image
                Image image = new()
                {
                    Width = Math.Max(obj.Width * 10, 5),
                    Height = Math.Max(obj.Height * 10, 5),
                    Source = new BitmapImage(new Uri("C:\\Users\\Erfan\\source\\Simulation\\Simulation.App\\Images\\smile.png")),
                    Tag = obj
                };

                Canvas.SetLeft(image, obj.Location.X);
                Canvas.SetTop(image, obj.Location.Y);

                image.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
                SimulationCanvas.Children.Add(image);

                DrawSearchRange(obj);
            }
        }


        //private void DrawObjects()
        //{
        //    SimulationCanvas.Children.Clear();



        //    //Debug.WriteLine($"EdibleObjects Count: {world.EdibleObjects.Count}");
        //    //Debug.WriteLine($"Entities Count: {world.Entities.Count}");

        //    foreach (var obj in world.EdibleObjects)
        //    {
        //        //Debug.WriteLine($"Drawing Edible: X={obj.Location.X}, Y={obj.Location.Y}, Width={obj.Width}, Height={obj.Height}");
        //        Ellipse shape = new()
        //        {
        //            Width = Math.Max(obj.Width * 10, 5),
        //            Height = Math.Max(obj.Height * 10, 5),
        //            Fill = Brushes.Green,
        //            Tag = obj
        //        };

        //        Canvas.SetLeft(shape, obj.Location.X);
        //        Canvas.SetTop(shape, obj.Location.Y);

        //        shape.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
        //        SimulationCanvas.Children.Add(shape);
        //    }
        //    if (world.Entities.Count > 1)
        //    {

        //    }
        //    foreach (var obj in world.Entities)
        //    {
        //        //Debug.WriteLine($"Drawing Entity: X={obj.Location.X}, Y={obj.Location.Y}, Width={obj.Width}, Height={obj.Height}");
        //        Ellipse shape = new()
        //        {
        //            Width = Math.Max(obj.Width * 10, 5),
        //            Height = Math.Max(obj.Height * 10, 5),
        //            Fill = Brushes.Blue,
        //            Tag = obj
        //        };

        //        Canvas.SetLeft(shape, obj.Location.X);
        //        Canvas.SetTop(shape, obj.Location.Y);

        //        shape.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
        //        SimulationCanvas.Children.Add(shape);


        //        DrawSearchRange(obj);

        //    }
        //}

        private void DrawSearchRange(LivingBeing entity)
        {
            // رسم دایره محدوده جستجو
            Ellipse searchRange = new()
            {
                Width = entity.VisualRange * 2, // قطر دایره
                Height = entity.VisualRange * 2,
                Stroke = Brushes.Red, // رنگ قرمز برای دایره محدوده جستجو
                StrokeThickness = 2,
                Opacity = 0.5, // شفافیت
                IsHitTestVisible = false // این دایره نباید کلیک را دریافت کند
            };

            // تنظیم موقعیت دایره جستجو (مرکز آن باید روی موجود قرار بگیرد)
            Canvas.SetLeft(searchRange, entity.Location.X - entity.VisualRange);
            Canvas.SetTop(searchRange, entity.Location.Y - entity.VisualRange);

            SimulationCanvas.Children.Add(searchRange);
        }


        Guid selectedObjectId;
        private void Shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Ellipse shape && shape.Tag is PhysicalObject obj)
            {
                selectedObjectId = obj.Id;
                SelectedObject = obj; // نمایش شیء انتخاب‌شده در PropertyGrid
            }
        }

        private async void StartSimulation()
        {
            isRunning = true;
            while (isRunning)
            {
                await Task.Delay(50);
                await world.UpdateAsync();


                DrawObjects();
            }
        }

        // 📌 زوم با اسکرول ماوس
        private void ScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double zoomDelta = (e.Delta > 0) ? zoomStep : -zoomStep;
            double newZoomFactor = zoomFactor + zoomDelta;

            if (newZoomFactor < minZoom || newZoomFactor > maxZoom)
                return;

            zoomFactor = newZoomFactor;

            // مرکز زوم را روی مکان ماوس تنظیم می‌کنیم
            Point mousePosition = e.GetPosition(SimulationCanvas);
            ZoomTransform.CenterX = mousePosition.X;
            ZoomTransform.CenterY = mousePosition.Y;
            ZoomTransform.ScaleX = zoomFactor;
            ZoomTransform.ScaleY = zoomFactor;
            //double zoomDelta = e.Delta > 0 ? zoomFactor : 1 / zoomFactor;

            //Point mousePosition = e.GetPosition(SimulationCanvas);
            //double oldScaleX = ZoomTransform.ScaleX;
            //double oldScaleY = ZoomTransform.ScaleY;

            //double newScaleX = oldScaleX * zoomDelta;
            //double newScaleY = oldScaleY * zoomDelta;

            //// محدود کردن زوم بین 0.5x و 5x
            //if (newScaleX < 0.5 || newScaleX > 5)
            //    return;

            //ZoomTransform.ScaleX = newScaleX;
            //ZoomTransform.ScaleY = newScaleY;

            //double offsetX = (mousePosition.X - TranslateTransform.X) * (zoomDelta - 1);
            //double offsetY = (mousePosition.Y - TranslateTransform.Y) * (zoomDelta - 1);

            //TranslateTransform.X -= offsetX;
            //TranslateTransform.Y -= offsetY;

            // جلوگیری از انتقال اسکرول به ScrollViewer
            e.Handled = true;  // 🚀 این خط باعث می‌شود که اسکرول صفحه به بالا و پایین نرود!
        }


        private void DrawGrid()
        {
            int gridSize = 50; // اندازه هر واحد
            for (int x = 0; x < 2000; x += gridSize)
            {
                Line verticalLine = new Line
                {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = 2000,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 0.5
                };
                SimulationCanvas.Children.Add(verticalLine);
            }

            for (int y = 0; y < 2000; y += gridSize)
            {
                Line horizontalLine = new Line
                {
                    X1 = 0,
                    Y1 = y,
                    X2 = 2000,
                    Y2 = y,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 0.5
                };
                SimulationCanvas.Children.Add(horizontalLine);
            }
        }

        // 📌 درگ کلیک راست برای حرکت در نقشه
        private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            lastMousePosition = e.GetPosition(this);
            isDragging = true;
            SimulationCanvas.CaptureMouse();
        }

        private void Canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            SimulationCanvas.ReleaseMouseCapture();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newMousePosition = e.GetPosition(this);
                double offsetX = newMousePosition.X - lastMousePosition.X;
                double offsetY = newMousePosition.Y - lastMousePosition.Y;

                TranslateTransform.X += offsetX;
                TranslateTransform.Y += offsetY;

                lastMousePosition = newMousePosition;
            }
        }

    }
}
