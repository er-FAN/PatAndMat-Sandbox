using Simulation.Engine.models;


namespace Simulation.Engine.events
{
    public interface ISimulationEvent
    {
        public string Type { get; set; }
        public ISimulableObject Source { get; set; }
    }
}
