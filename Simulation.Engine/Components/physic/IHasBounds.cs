using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.Components.physic
{
    public interface IHasBounds : IComponent
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
    }

}
