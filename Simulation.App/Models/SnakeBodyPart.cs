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

namespace Simulation.App.Models
{

    public class SnakeBodyPart : ISimulableObject, ITaggable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<ILogic> Logics { get; set; } = new();
        public List<ISimulationEvent> Events { get; set; } = new();
        public List<IEventListener> Listeners { get; set; } = new();
        public List<IComponent> Components { get; set; } = new();

        public Direction Direction { get; set; } = Direction.Right;
        public string? Tag { get; set; }

        public bool isHead = false;

        public SnakeBodyPart(int x, int y)
        {
            AddBoundingBoxComponent(x, y);
            AddUpdateRenderLogic();
            AddGetNextSpriteLogic();

        }

        private void AddGetNextSpriteLogic()
        {
            Logics.Add(new GetNextSpriteLogic("walk"));
        }

        private void AddUpdateRenderLogic()
        {
            Logics.Add(new UpdateRenderLogic());
        }

        private void AddBoundingBoxComponent(int x, int y)
        {
            Components.Add(new BoundingBoxComponent()
            {
                Position = new Vector2(x, y),
                Size = new Vector2(20, 20)
            });
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