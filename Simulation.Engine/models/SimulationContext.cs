using Simulation.Engine.events;

namespace Simulation.Engine.models
{
    public class SimulationContext : IContext
    {
        public List<IContext> ChildContexts { get; set; } = [];
        public List<ISimulableObject> Objects { get; } = [];
        public List<ISimulationEvent> Events { get; set; } = [];
        public List<IEventListener> Listeners { get; set; } = [];
        public EventBus ContextEventBus { get; }
        public List<ILogic> Logics { get; set; } = [];

        public SimulationContext()
        {
            ContextEventBus = new(this);
        }

        public void Update()
        {
            for (int i = 0; i < ChildContexts.Count; i++)
            {
                ChildContexts[i].Update();
            }

            for (int i = 0; i < Objects.Count; i++)
            {
                for (int j = 0; j < Objects[i].Logics.Count; j++)
                {
                    Objects[i].Logics[j].Apply(Objects[i], this);
                }
            }
        }
    }

}