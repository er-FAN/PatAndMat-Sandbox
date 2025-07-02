using Simulation.Engine.Components;
using Simulation.Engine.events;
using Simulation.Engine.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Simulation.App.Models
{

    public class SnakeBodyPart : ISimulableObject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<ILogic> Logics { get; set; } = new();
        public List<ISimulationEvent> Events { get; set; } = new();
        public List<IEventListener> Listeners { get; set; } = new();
        public List<IComponent> Components { get; set; } = new();

        // مختصات منطقی (grid) این قطعه
        public int X { get; set; }
        public int Y { get; set; }
        public bool isHead = false;

        public SnakeBodyPart(int x, int y, System.Drawing.Color color)
        {
            X = x;
            Y = y;

            var wpfRender = new WpfRender
            {
                SpriteId = "C:\\Users\\Erfan\\source\\Simulation\\Simulation.App\\images\\snake\\snake_green_blob.png",
                Color = color,
                Size = new Vector2(25f, 25f),
                Position = new Vector2(x, y)
            };
            Components.Add(wpfRender);
            WpfAnimatable wpfAnimatable = new WpfAnimatable();
            List<string> imagePaths =
                        [
                            "C:\\Users\\Erfan\\source\\Simulation\\Simulation.App\\images\\snake\\snake_green_head.png",
                            "C:\\Users\\Erfan\\source\\Simulation\\Simulation.App\\images\\snake\\snake_green_head.png",
                "C:\\Users\\Erfan\\source\\Simulation\\Simulation.App\\images\\snake\\snake_green_eyes.png"
            ];
            wpfAnimatable.Sprites.Add("walk", imagePaths);

            Components.Add(wpfAnimatable);

            // تنها منطق لازم، آپدیت موقعیت رندر است
            Logics.Add(new UpdateRenderLogic());
            Logics.Add(new GetNextSpriteLogic("walk"));
        }

        // Wrapper برای دسترسی آسان‌تر به کامپوننت‌ها
        public T? GetComponent<T>() where T : class, IComponent
        {
            return Components.OfType<T>().FirstOrDefault();
        }

        T? ISimulableObject.GetComponent<T>() where T : class
        {
            return Components.OfType<T>().FirstOrDefault();
        }
    }


    // لوجیک عمومی برای آپدیت کردن Position در WpfRender
    public class UpdateRenderLogic : ILogic
    {
        public void Apply(ISimulableObject obj, IContext ctx)
        {
            if (obj is SnakeBodyPart part &&
                part.GetComponent<WpfRender>() is WpfRender r && part.GetComponent<WpfAnimatable>() is WpfAnimatable a)
            {
                if (part.isHead)
                {
                    r.SpriteId = a.CurrentSpriteAddres;
                }
                else
                {
                    r.SpriteId = "C:\\Users\\Erfan\\source\\Simulation\\Simulation.App\\images\\snake\\snake_green_blob.png";
                }
                r.Position = new Vector2(part.X, part.Y);
            }
        }
    }

    public class GetNextSpriteLogic(string state) : ILogic
    {
        public string State = state;
        WpfAnimatable Animatable = new();
        public void Apply(ISimulableObject simulableObject, IContext context)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            Animatable = simulableObject.GetComponent<WpfAnimatable>();
#pragma warning restore CS8601 // Possible null reference assignment.
            if (Animatable != null)
            {
                Animatable.CurrentSpriteAddres = Animatable.Sprites[State].ToList()[Animatable.CurrentSpriteIndex];
                Animatable.CurrentSprite = LoadImage(Animatable.CurrentSpriteAddres);
                Animatable.CurrentSpriteIndex++;

                if (Animatable.CurrentSpriteIndex >= Animatable.Sprites[State].Count)
                {
                    Animatable.CurrentSpriteIndex = 0;
                }
            }
        }

        BitmapImage LoadImage(string imagePath)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
            bitmap.EndInit();

            return bitmap;
        }
    }
}
