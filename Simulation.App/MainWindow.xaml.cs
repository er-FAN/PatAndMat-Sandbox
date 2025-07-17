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
using System.Numerics;

namespace Simulation.App
{


    public partial class MainWindow : Window
    {
        //private readonly List<IRenderable> _objects = new();
        private readonly IContext mainContext;
        private readonly DispatcherTimer _timer;
        List<IRenderable> renderables;

        public MainWindow()
        {
            InitializeComponent();

            mainContext = new SimulationContext();

            InitializeGame();

            CompositionTarget.Rendering += OnRenderFrame;
        }

        private void InitializeGame()
        {
            GameGroundContext gamegroundContext = new GameGroundContext();
            mainContext.ChildContexts.Add(gamegroundContext);
            InitializeSnake(gamegroundContext);
            InitializeWalls(gamegroundContext);
        }

        private static void InitializeSnake(GameGroundContext gamegroundContext)
        {
            SnakeBodyContext snake = new SnakeBodyContext(startX: 300, startY: 40, initialLength: 20, color: System.Drawing.Color.Blue);
            gamegroundContext.ChildContexts.Add(snake);
        }

        private static void InitializeWalls(GameGroundContext gamegroundContext)
        {
            Wall wall1 = new Wall(new Vector2(720, 10), new Vector2(20, 20), Direction.Up);
            wall1.Components.Add(new BoundingBoxComponent
            {
                Position = wall1.Position,
                Size = wall1.Size
            });
            Wall wall2 = new Wall(new Vector2(10, 300), new Vector2(720, 20), Direction.Right);
            wall2.Components.Add(new BoundingBoxComponent
            {
                Position = wall2.Position,
                Size = wall2.Size
            });
            Wall wall3 = new Wall(new Vector2(700, 10), new Vector2(20, 300), Direction.Down);
            wall3.Components.Add(new BoundingBoxComponent
            {
                Position = wall3.Position,
                Size = wall3.Size
            });
            Wall wall4 = new Wall(new Vector2(10, 300), new Vector2(20, 20), Direction.Left);
            wall4.Components.Add(new BoundingBoxComponent
            {
                Position = wall4.Position,
                Size = wall4.Size
            });

            gamegroundContext.Objects.Add(wall1);
            gamegroundContext.Objects.Add(wall2);
            gamegroundContext.Objects.Add(wall3);
            gamegroundContext.Objects.Add(wall4);
        }

        private void OnRenderFrame(object sender, EventArgs e)
        {
            mainContext.Update();

            new Thread(() =>
            {
                renderables = [.. CollectRenderables(mainContext).OrderBy(r => r.Layer)];
            }).Start();

            RenderCanvas.Children.Clear();
            if (renderables != null)
            {
                foreach (IRenderable obj in renderables)
                {
                    if (obj is TiledRender tiled)
                    {
                        var sprite = new BitmapImage(new Uri(tiled.SpriteId, UriKind.RelativeOrAbsolute));

                        int tilesX = (int)Math.Ceiling(tiled.Size.X / tiled.TileSize.X);
                        int tilesY = (int)Math.Ceiling(tiled.Size.Y / tiled.TileSize.Y);

                        for (int x = 0; x < tilesX; x++)
                        {
                            for (int y = 0; y < tilesY; y++)
                            {
                                var image = new Image
                                {
                                    Width = tiled.TileSize.X,
                                    Height = tiled.TileSize.Y,
                                    Source = sprite
                                };

                                Canvas.SetLeft(image, tiled.Position.X + x * tiled.TileSize.X);
                                Canvas.SetTop(image, tiled.Position.Y + y * tiled.TileSize.Y);

                                RenderCanvas.Children.Add(image);
                            }
                        }
                    }

                    if(obj is WpfRender)
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

        private void Key_Down(object sender, KeyEventArgs e)
        {
            string input = "user_input.any";
            input = CheckUserInput(e, input);
            UserInputEvent inputevent = new UserInputEvent();
            inputevent.Type = input;
            mainContext.ContextEventBus.Publish(inputevent);
        }

        private static string CheckUserInput(KeyEventArgs e, string input)
        {
            if (e.Key == Key.A)
            {
                input = "user_input.A";
            }
            if (e.Key == Key.D)
            {
                input = "user_input.D";
            }
            if (e.Key == Key.S)
            {
                input = "user_input.S";
            }
            if (e.Key == Key.W)
            {
                input = "user_input.W";
            }
            if (e.Key == Key.Space)
            {
                input = "user_input.space";
            }

            return input;
        }

        public class UserInputEvent : ISimulationEvent
        {
            public string Type { get; set; }
            public ISimulableObject Source { get; set; }
        }

        public static List<IRenderable> CollectRenderables(IContext context)
        {
            var result = new List<IRenderable>();

            foreach (var obj in context.Objects)
            {
                var render = obj.GetComponent<IRenderable>();
                if (render != null)
                    result.Add(render);
            }

            foreach (var child in context.ChildContexts)
            {
                result.AddRange(CollectRenderables(child));
            }

            return result;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            mainContext.Update();

            // پاک کردن رسم قبلی
            RenderCanvas.Children.Clear();

            List<IRenderable> renderables = CollectRenderables(mainContext);



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
