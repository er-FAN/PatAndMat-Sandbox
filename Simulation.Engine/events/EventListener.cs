

using Simulation.Engine.models;

namespace Simulation.Engine.events
{
    public interface IEventListener
    {
        public IContext Context { get; set; }
        public ISimulableObject SimulableObject { get; set; }
        bool ShouldListen(ISimulationEvent e);
        void OnEvent(ISimulationEvent e);
    }
}
