using Simulation.Engine.models;
using System.Runtime.InteropServices.Marshalling;

namespace Simulation.Engine.events
{
    public class EventBus
    {
        private readonly List<ISimulableObject> allObjects;

        public EventBus(List<ISimulableObject> objects)
        {
            allObjects = objects;
        }

        public void Publish(ISimulationEvent e)
        {
            foreach (var obj in allObjects)
            {
                foreach (var listener in obj.EventListeners)
                {
                    if (listener.ShouldListen(e))
                    {
                        listener.OnEvent(e);
                    }
                }
            }
        }
    }

}