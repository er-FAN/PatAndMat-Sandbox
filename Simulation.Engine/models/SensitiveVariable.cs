using Simulation.Engine.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.models
{
    public class SensitiveVariable<T, E> where E : class, IConstructible<T>, new()
    {
        private T _value;
        private T oldValue;
        public event EventHandler<E> EventHandler;
        public SensitiveVariable(T initialValue, EventHandler<E> eventHandler)
        {
            oldValue = initialValue;
            _value = initialValue;
            EventHandler = eventHandler;
        }

        public T Value
        {
            get { return _value; }
            set
            {
                oldValue = _value;
                _value = value;
                E eventArgs = new E();
                eventArgs.Construct(oldValue, _value);
                EventHandler.Invoke(this, eventArgs);
            }
        }
    }
}
