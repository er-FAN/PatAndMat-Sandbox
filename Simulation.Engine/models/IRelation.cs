

namespace Simulation.Engine.models
{
    public interface IRelation
    {
        public string Type { get; set; } // مثل contains, connectedTo, parentOf...
        public ISimulableObject Source { get; set; }
        public ISimulableObject Target { get; set; }
    }

}
