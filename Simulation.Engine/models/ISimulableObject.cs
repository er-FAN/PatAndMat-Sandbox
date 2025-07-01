using Simulation.Engine.Components;
using Simulation.Engine.events;

namespace Simulation.Engine.models
{
    public interface ISimulableObject
    {
        public Guid Id { get; set; }
        public List<ILogic> Logics { get; set; }
        public List<ISimulationEvent> Events { get; set; }
        public List<IEventListener> Listeners { get; set; }
        public List<IComponent> Components { get; set; }

        public T? GetComponent<T>() where T : class, IComponent;
    }
}
