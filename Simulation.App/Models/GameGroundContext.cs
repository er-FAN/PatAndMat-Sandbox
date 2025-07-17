using Simulation.Engine.events;
using Simulation.Engine.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.App.Models
{
    public class GameGroundContext : IContext
    {
        public List<ILogic> Logics { get; set; } = [];
        public List<IContext> ChildContexts { get; set; } = [];

        public List<ISimulableObject> Objects { get; set; } = [];

        public List<ISimulationEvent> Events { get; set; } = [];
        public List<IEventListener> Listeners { get; set; } = [];

        public EventBus ContextEventBus { get; }

        public GameGroundContext()
        {
            ContextEventBus = new(this);
        }


        public void Update()
        {
            foreach (var context in ChildContexts)
            {
                context.Update();
            }

            foreach (var logic in Logics)
            {
                logic.Apply(null, this);
            }

            foreach (var obj in Objects)
                foreach (var logic in obj.Logics)
                    logic.Apply(obj, this);
        }
    }


    
}
