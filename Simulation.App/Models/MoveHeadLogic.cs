using Simulation.Engine.models;
using System.Numerics;

namespace Simulation.App.Models
{
    public class MoveHeadLogic : ILogic
    {
        public void Apply(ISimulableObject self, IContext ctx)
        {
            var bb = self.GetComponent<BoundingBoxComponent>();
            if (bb == null) return;
            if (self is not SnakeBodyPart head) return;
            switch (head.Direction)
            {
                case Direction.Up: bb.Position = new Vector2(bb.Position.X, bb.Position.Y - 1); break;
                case Direction.Down: bb.Position = new Vector2(bb.Position.X, bb.Position.Y + 1); break;
                case Direction.Left: bb.Position = new Vector2(bb.Position.X - 1, bb.Position.Y); break;
                case Direction.Right: bb.Position = new Vector2(bb.Position.X + 1, bb.Position.Y); ; break;
            }
        }
    }

}