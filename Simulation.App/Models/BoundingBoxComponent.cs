using System.Numerics;
using Simulation.Engine.Components.physic;
using Simulation.Engine.models;

namespace Simulation.App.Models
{
    public class BoundingBoxComponent : IHasBounds
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public List<ILogic> Logics { get; set; }
    }

}
