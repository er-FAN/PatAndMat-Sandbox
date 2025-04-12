

using Simulation.Engine.events;

namespace Simulation.Engine.models
{
    public class SimulationContext
    {
        public List<ISimulableObject> Objects { get; } = new();
        public List<IRule> Rules { get; } = new();
        public EventBus ContextEventBus { get; } = new();
    }

}