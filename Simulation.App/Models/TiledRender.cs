using Simulation.Engine.models;
using Simulation.Engine.Components.Render;
using System.Numerics;

namespace Simulation.App.Models
{
    public class TiledRender : IRenderable
    {
        public string SpriteId { get; set; } = string.Empty;
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public System.Drawing.Color Color { get; set; } = System.Drawing.Color.White;

        // اندازه‌ی اسپرایت اصلی که باید تکرار شود
        public Vector2 TileSize { get; set; } = new Vector2(32, 32);
        public List<ILogic> Logics { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Layer { get; set; } = 0;
    }

}
