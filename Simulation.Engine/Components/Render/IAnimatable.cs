using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.Components.Render
{
    public interface IAnimatable : IComponent
    {
        public Dictionary<string,List<string>> Sprites { get; set; }
    }
}
