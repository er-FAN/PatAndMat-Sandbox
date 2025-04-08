using Simulation.Engine.events;

namespace Simulation.Engine.models
{
    public interface ISimulableObject: IHasBehaviors, IHasRelations, IHasEventListener
    {
        public Guid Id { get; set; }
    }
}
