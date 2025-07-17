using Simulation.Engine.models;
using System.Runtime.InteropServices.Marshalling;

namespace Simulation.Engine.events
{
    public class EventBus(IContext context)
    {
        public IContext Context { get; private set; } = context;

        public void Publish(ISimulationEvent e)
        {
            PublishInChildContexts(e);
            PublishInObjects(e);
        }

        private void PublishInChildContexts(ISimulationEvent e)
        {
            foreach (IContext ctx in Context.ChildContexts)
            {
                ctx.ContextEventBus.Publish(e);
                foreach (IEventListener listener in ctx.Listeners)
                {
                    if (listener.ShouldListen(e))
                    {
                        listener.OnEvent(e);
                    }
                }
            }
        }

        private void PublishInObjects(ISimulationEvent e)
        {
            foreach (ISimulableObject obj in Context.Objects)
            {
                foreach (IEventListener listener in obj.Listeners)
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