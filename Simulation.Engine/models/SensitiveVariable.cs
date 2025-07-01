using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.models
{
    public class SensitiveVariable<T>
    {
        private T value;
        private readonly List<(Predicate<T>, Action<T>)> reactions = new();

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                foreach (var (condition, action) in reactions)
                {
                    if (condition(value))
                        action(value);
                }
            }
        }

        public void AddReaction(Predicate<T> condition, Action<T> action)
        {
            reactions.Add((condition, action));
        }

        public SensitiveVariable(T initialValue)
        {
            value = initialValue;
        }
    }

}
