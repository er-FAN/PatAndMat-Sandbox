using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.events
{
    public class SleepChangedEventArgs : EventArgs, IConstructible<int>
    {
        public int AddedSleepValue { get; private set; }
        public int CurrentBeingSleep { get; private set; }
        public SleepChangedEventArgs(int addedSleepValue,int currentBeingSleep) 
        {
            AddedSleepValue = addedSleepValue;
            CurrentBeingSleep = currentBeingSleep;
        }

        public SleepChangedEventArgs()
        {
            
        }

        public void Construct(int param1, int param2)
        {
            AddedSleepValue = param1;
            CurrentBeingSleep = param2;
        }
    }
}
