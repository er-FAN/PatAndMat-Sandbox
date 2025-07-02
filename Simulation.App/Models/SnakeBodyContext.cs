using Simulation.Engine.events;
using Simulation.Engine.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
        public List<ISimulationEvent> Events { get; set; } = [];
        public List<IEventListener> Listeners { get; set; } = [];

        public Direction Direction { get; set; } = Direction.Right;

        public List<ISimulableObject> Objects { get; set; } = [];
        public List<ILogic> Logics { get; set; } = [];

        public bool growNext = false;

        public SnakeBodyContext(int startX, int startY, int initialLength, System.Drawing.Color color)
        {
            ContextEventBus = new(this);
            // ایجاد مار اولیه به طول initialLength به صورت خط افقی
            for (int i = 0; i < initialLength; i++)
            {
                var part = new SnakeBodyPart(startX - i, startY, color);
                body.AddLast(part);
            }
            Listeners.Add(new InputEventListener(this, null));
            Logics.Add(new MoveSnakeLogic());
        }

        // اگر سر غذا بخورد، این را true کنید قبل از Update
        public void Grow() => growNext = true;

        public void Update()
        {
            //MoveSnake();

            foreach (var logic in Logics)
            {
                logic.Apply(null, this);
            }

            foreach (var part in body)
                foreach (var logic in part.Logics)
                    logic.Apply(part, this);
        }
    }

    public class MoveSnakeLogic : ILogic
    {
        int step = 1;
        public void Apply(ISimulableObject simulableObject, IContext context)
        {
            step++;
            if (context is SnakeBodyContext snake && step >= 7)
            {
                step = 0;

                var head = snake.body.First!.Value;
                int newX = head.X, newY = head.Y;

                switch (snake.Direction)
                {
                    case Direction.Up: newY -= 20; break;
                    case Direction.Down: newY += 20; break;
                    case Direction.Left: newX -= 20; break;
                    case Direction.Right: newX += 20; break;
                }

                if (snake.growNext)
                {
                    // ۲-۱) اگر باید بزرگ شود: یک قطعهٔ جدید بسازیم و AddFirst کنیم
                    var newHead = new SnakeBodyPart(
                        newX, newY,
                        snake.body.First!.Value.GetComponent<WpfRender>()!.Color
                    );
                    snake.body.AddFirst(newHead);
                    snake.growNext = false;
                }
                else
                {
                    // ۲-۲) اگر نباید بزرگ شود: دم را برداریم و به سر بچسبانیم (حلقوی)
                    var tailPart = snake.body.Last!.Value;
                    snake.body.RemoveLast();

                    tailPart.X = newX;
                    tailPart.Y = newY;
                    snake.body.AddFirst(tailPart);
                }

                foreach (var body in snake.body)
                {
                    body.isHead = false;
                }
                snake.body.First.Value.isHead = true;
                //if(snake.body.Last.Value.GetComponent<WpfRender>() is WpfRender render)
                //{
                //    render.Size=new Vector2 (20, 20);
                //}

                snake.Objects = [];
                snake.Objects.AddRange(snake.body.Reverse());
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

    public class InputEventListener(IContext context, ISimulableObject simulableObject) : IEventListener
    {
        public IContext Context { get; set; } = context;
        public ISimulableObject SimulableObject { get; set; } = simulableObject;

        public void OnEvent(ISimulationEvent e)
        {
            string input = e.Type.Split('.')[1];
            if (Context is SnakeBodyContext snakeBodyContext)
            {
                switch (input)
                {
                    case "W":
                        if (snakeBodyContext.Direction != Direction.Down)
                        {
                            snakeBodyContext.Direction = Direction.Up;
                        }
                        break;
                    case "S":
                        if (snakeBodyContext.Direction != Direction.Up)
                        {
                            snakeBodyContext.Direction = Direction.Down;
                        }
                        break;
                    case "A":
                        if (snakeBodyContext.Direction != Direction.Right)
                        {
                            snakeBodyContext.Direction = Direction.Left;
                        }
                        break;
                    case "D":
                        if (snakeBodyContext.Direction != Direction.Left)
                        {
                            snakeBodyContext.Direction = Direction.Right;
                        }
                        break;
                    default:
                        break;
                }
            }

        }

        public bool ShouldListen(ISimulationEvent e)
        {
            bool resualt = false;
            if (e.Type.StartsWith("user_input"))
            {
                resualt = true;
            }
            return resualt;
        }
    }
}
