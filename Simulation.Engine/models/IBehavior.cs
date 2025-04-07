

namespace Simulation.Engine.models
{
    public interface IBehavior
    {
        void Apply(ISimulableObject obj, SimulationContext context);
    }

}
