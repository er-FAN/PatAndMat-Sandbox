using Simulation.Engine.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.models
{
    public interface IContext
    {
        public List<ILogic> Logics { get; set; }
        public List<IContext> ChildContexts { get; set; }
        public List<ISimulableObject> Objects { get; }
        public List<ISimulationEvent> Events { get; set; }
        public List<IEventListener> Listeners { get; set; }
        public EventBus ContextEventBus { get; }

        public void Update();
    }
}
