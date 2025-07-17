using Simulation.Engine.Components;
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
    public class Wall : ISimulableObject, ITaggable
    {
        public Guid Id { get; set; }
        public List<ILogic> Logics { get; set; } = [];
        public List<ISimulationEvent> Events { get; set; } = [];
        public List<IEventListener> Listeners { get; set; } = [];
        public List<IComponent> Components { get; set; } = [];

        public Vector2 Size { get; set; }
        public Vector2 Position { get; set; }
        public Direction Direction { get; set; }
        public string? Tag { get; set; } = "wall";

        public Wall(Vector2 size, Vector2 position,Direction direction)
        {
            Size = size;
            Position = position;
            Direction = direction;
            TiledRender render = new TiledRender
            {
                Position = position,
                Size = size,
                SpriteId = "C:\\Users\\Erfan\\source\\Simulation\\Simulation.App\\images\\snake\\wall_block_0.png",
                TileSize = new Vector2(20, 20), // سایز تکه‌های اسپرایت
                Color = System.Drawing.Color.Black
            };

            Components.Add(render);
            //Components.Add(new BoundingBoxComponent());
            AddCollisionLogic(this);
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

        private string? GetTag(ISimulableObject obj)
        {
            if (obj is ITaggable taggable)
                return taggable.Tag;
            return null;
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
}
