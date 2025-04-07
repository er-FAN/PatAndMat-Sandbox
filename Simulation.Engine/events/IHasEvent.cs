using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.events
{
    public interface IHasEvent
    {
        public EventBus EventBus { get; set; }
        public List<ISimulationEvent> Events { get; set; }
    }
}
