using Simulation.App.Models;
using Simulation.Engine.events;
using Simulation.Engine.models;

public class InputEventListener(ISimulableObject simulableObject) : IEventListener
{
    public IContext Context { get; set; }
    public ISimulableObject SimulableObject { get; set; } = simulableObject;

    public void OnEvent(ISimulationEvent e)
    {
        string input = e.Type.Split('.')[1];
        if (simulableObject is SnakeBodyPart snake && Context is SnakeBodyContext ctx)
        {
            switch (input)
            {
                case "W":
                    if (snake.Direction != Direction.Down)
                    {
                        snake.Direction = Direction.Up;
                    }
                    break;
                case "S":
                    if (snake.Direction != Direction.Up)
                    {
                        snake.Direction = Direction.Down;
                    }
                    break;
                case "A":
                    if (snake.Direction != Direction.Right)
                    {
                        snake.Direction = Direction.Left;
                    }
                    break;
                case "D":
                    if (snake.Direction != Direction.Left)
                    {
                        snake.Direction = Direction.Right;
                    }
                    break;
                //case "space":
                //    ctx.Grow();
                //    break;
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

