using Simulation.Engine.models;
using System.Runtime.InteropServices.Marshalling;

namespace Simulation.Engine.events
{
    public class EventBus
    {
        public List<ISimulableObject> allObjects = [];

        public void Publish(ISimulationEvent e)
        {
            foreach (var obj in allObjects)
            {
                if (obj is IHasEventListener eventListener)
                {
                    foreach (var listener in eventListener.EventListeners)
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

}