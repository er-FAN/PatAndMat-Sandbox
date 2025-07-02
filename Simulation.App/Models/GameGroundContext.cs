using Simulation.Engine.events;
using Simulation.Engine.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.App.Models
{
    public class GameGroundContext : IContext
    {
        public List<ILogic> Logics { get; set; } = [];
        public List<IContext> ChildContexts { get; set; } = [];

        public List<ISimulableObject> Objects { get; set; } = [];

        public List<ISimulationEvent> Events { get; set; } = [];
        public List<IEventListener> Listeners { get; set; } = [];

        public EventBus ContextEventBus { get; }

        public GameGroundContext()
        {
            ContextEventBus = new(this);
            Logics.Add(new CollisionLogic());
        }


        public void Update()
        {
            foreach (var context in ChildContexts)
            {
                context.Update();
            }

            foreach (var logic in Logics)
            {
                logic.Apply(null, this);
            }

            foreach (var obj in Objects)
                foreach (var logic in obj.Logics)
                    logic.Apply(obj, this);
        }
    }

    public class CollisionLogic : ILogic
    {
        public void Apply(ISimulableObject simulableObject, IContext context)
        {
            SnakeBodyContext? snakeBodyContext = null;
            SnakeBodyPart? snakeBodyPart = null;
            if (context is GameGroundContext gamegroundContext)
            {
                foreach (var item in gamegroundContext.ChildContexts)
                {
                    if (item is SnakeBodyContext snake)
                    {
                        snakeBodyContext = snake;
                        foreach (var item2 in snake.Objects)
                        {
                            if (item2 is SnakeBodyPart sbp)
                            {
                                if (sbp.isHead)
                                {
                                    snakeBodyPart = sbp;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
                foreach (var item in gamegroundContext.Objects)
                {
                    if (item is Wall w && snakeBodyContext != null && snakeBodyPart != null)
                    {
                        if (snakeBodyContext.Direction == Direction.Up && w.Direction == Direction.Up)
                        {
                            if (snakeBodyPart.Y <= w.Position.Y)
                            {
                                Collision(snakeBodyContext, snakeBodyPart);
                            }
                        }
                        if(snakeBodyContext.Direction == Direction.Down && w.Direction == Direction.Down)
                        {
                            if (snakeBodyPart.Y >= w.Position.Y)
                            {
                                Collision(snakeBodyContext, snakeBodyPart);
                            }
                        }
                        if (snakeBodyContext.Direction == Direction.Left && w.Direction == Direction.Left)
                        {
                            if (snakeBodyPart.X <= w.Position.X)
                            {
                                Collision(snakeBodyContext, snakeBodyPart);
                            }
                        }
                        if(snakeBodyContext.Direction == Direction.Right && w.Direction == Direction.Right)
                        {
                            if (snakeBodyPart.X >= w.Position.X)
                            {
                                Collision(snakeBodyContext, snakeBodyPart);
                            }
                        }
                    }
                }
            }
        }

        private static void Collision(SnakeBodyContext? snakeBodyContext, SnakeBodyPart? snakeBodyPart)
        {
            snakeBodyContext.Logics = [];
            snakeBodyContext.Listeners = [];
            snakeBodyPart.GetComponent<WpfRender>().SpriteId =
                "C:\\Users\\Erfan\\source\\Simulation\\Simulation.App\\images\\snake\\snake_green_xx.png";
        }
    }
}
