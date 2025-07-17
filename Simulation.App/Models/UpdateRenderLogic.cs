using Simulation.Engine.models;
using System.Numerics;

namespace Simulation.App.Models
{
    // لوجیک عمومی برای آپدیت کردن Position در WpfRender
    public class UpdateRenderLogic : ILogic
    {
        int step = 0;
        public void Apply(ISimulableObject obj, IContext ctx)
        {
            step++;
            if (obj is SnakeBodyPart part && step >= 7 &&
                part.GetComponent<WpfRender>() is WpfRender r
                && part.GetComponent<BoundingBoxComponent>() is BoundingBoxComponent b)
            {
                step = 0;
                if (part.isHead && part.GetComponent<WpfAnimatable>() is WpfAnimatable a)
                {
                    r.SpriteId = a.CurrentSpriteAddres;
                }
                else
                {
                    r.SpriteId = "C:\\Users\\Erfan\\source\\Simulation\\Simulation.App\\images\\snake\\snake_green_blob.png";
                }
                r.Position = new Vector2(b.Position.X, b.Position.Y);
            }
        }
    }

}