using Simulation.Engine.Components.physic;
using Simulation.Engine.events;
using Simulation.Engine.models;
using System.Drawing;
using System.Numerics;

namespace Simulation.App.Models
{

    public class SnakeBodyContext : IContext
    {
        public List<IContext> ChildContexts { get; set; } = new();
        public EventBus ContextEventBus { get; }

        public SnakeBodyContext()
        {
            ContextEventBus = new(this);
        }

        // لیست دوطرفه برای قطعات بدن، از سر (First) تا دم (Last)
        public LinkedList<SnakeBodyPart> body = new();
        private Dictionary<SnakeBodyPart, Vector2> previousPositions = new();
        public List<ISimulationEvent> Events { get; set; } = [];
        public List<IEventListener> Listeners { get; set; } = [];



        public List<ISimulableObject> Objects { get; set; } = [];
        public List<ILogic> Logics { get; set; } = [];

        public bool growNext = false;
        int lastLayer = 10000;
        public SnakeBodyContext(int startX, int startY, int initialLength, System.Drawing.Color color)
        {

            ContextEventBus = new(this);

            SnakeBodyPart? prevPart = null;

            for (int i = 0; i < initialLength; i++)
            {
                var part = new SnakeBodyPart(40, startY);
                if (i == 0)
                {
                    
                    InitializeHeadPart(startX, startY, part);
                    AddCollisionLogic(part);
                }
                else if (prevPart != null)
                {
                    AddFollowTargetLogic(prevPart, part);
                    AddRenderComponent(new Vector2(startX, startY), part);

                }

                prevPart = part;
                part.Tag = "snake";
                body.AddLast(part);
            }

            Objects.AddRange(body);

        }

        private void AddCollisionLogic(ISimulableObject part)
        {
            var collisionLogic = new BoundsCollisionLogic
            {
                TagCondition = (tagA, tagB) =>
                {
                    // فقط اگر یکی Snake و دیگری Wall باشد برخورد بررسی شود
                    return (tagA == "snake" && tagB == "wall") || (tagA == "wall" && tagB == "snake");
                },
                OnCollision = (a, b) =>
                {
                    Console.WriteLine($"Collision between {GetTag(a)} and {GetTag(b)}");
                }
            };
            part.Logics.Add(collisionLogic);
        }

        string GetTag(ISimulableObject obj)
        {
            if (obj is ITaggable taggable)
            {
                return taggable.Tag;
            }
            return string.Empty;
        }

        private static void AddFollowTargetLogic(SnakeBodyPart? prevPart, SnakeBodyPart part)
        {
            part.Logics.Add(new FollowTargetLogic(prevPart));
        }

        private void InitializeHeadPart(int startX, int startY, SnakeBodyPart part)
        {
            AddRenderComponent(new Vector2(startX, startY), part);
            AddWpfAnimatableComponent(part);
            part.isHead = true;
            AddMoveHeadLogic(part);
            AddInputEventListener(part);
        }

        private void AddInputEventListener(SnakeBodyPart part)
        {
            Listeners.Add(new InputEventListener(part) { Context = this });
        }

        private static void AddMoveHeadLogic(SnakeBodyPart part)
        {
            part.Logics.Add(new MoveHeadLogic());
        }

        private static void AddWpfAnimatableComponent(SnakeBodyPart part)
        {
            WpfAnimatable wpfAnimatable = new WpfAnimatable();
            List<string> imagePaths =
                        [
                            "C:\\Users\\Erfan\\source\\Simulation\\Simulation.App\\images\\snake\\snake_green_head.png",
                                    "C:\\Users\\Erfan\\source\\Simulation\\Simulation.App\\images\\snake\\snake_green_head.png",
                                    "C:\\Users\\Erfan\\source\\Simulation\\Simulation.App\\images\\snake\\snake_green_eyes.png"
            ];
            wpfAnimatable.Sprites.Add("walk", imagePaths);

            part.Components.Add(wpfAnimatable);
        }

        private void AddRenderComponent(Vector2 position, SnakeBodyPart part)
        {
            var wpfRender = new WpfRender
            {
                SpriteId = "C:\\Users\\Erfan\\source\\Simulation\\Simulation.App\\images\\snake\\snake_green_blob.png",
                Size = new Vector2(25f, 25f),
                Position = position,
                Layer = lastLayer
            };
            part.Components.Add(wpfRender);
            lastLayer--;
        }

        // اگر سر غذا بخورد، این را true کنید قبل از Update
        public void Grow()
        {
            SnakeBodyPart bodyPart = new SnakeBodyPart(0, 0);
            var wpfRender = new WpfRender
            {
                SpriteId = "C:\\Users\\Erfan\\source\\Simulation\\Simulation.App\\images\\snake\\snake_green_blob.png",
                Color = System.Drawing.Color.Beige,
                Size = new Vector2(25f, 25f),
                Position = new Vector2(0, 0),
                Layer = lastLayer
            };
            bodyPart.Components.Add(wpfRender);
            lastLayer--;
            bodyPart.Logics.Add(new FollowTargetLogic(body.Last()));
            body.AddLast(bodyPart);
        }

        public void Update()
        {
            // آپدیت همه بخش‌ها
            foreach (var part in body)
                foreach (var logic in part.Logics)
                    logic.Apply(part, this);



            // تنظیم وضعیت اشیای کانتکست
            Objects = [];
            Objects.AddRange(body);
        }
    }
}



public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

