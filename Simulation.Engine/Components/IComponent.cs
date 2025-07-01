using Simulation.Engine.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.Components
{
    public interface IComponent 
    {
        public List<ILogic> Logics { get; set; }
    }
}
