using Simulation.Engine.Components.Render;
using Simulation.Engine.models;
using System.Numerics;

namespace Simulation.App.Models
{
    public class WpfRender : IRenderable
    {
        public string SpriteId { get; set; } = string.Empty;
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public System.Drawing.Color Color { get; set; }

        public List<ILogic> Logics { get; set; } = [];
    }
}
