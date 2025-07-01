using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.models
{
    public interface ILogic
    {
        void Apply(ISimulableObject simulableObject, IContext context);
    }
}
