

namespace Simulation.Engine.events
{
    public interface IEventListener
    {
        bool ShouldListen(ISimulationEvent e);
        void OnEvent(ISimulationEvent e);
    }
}
