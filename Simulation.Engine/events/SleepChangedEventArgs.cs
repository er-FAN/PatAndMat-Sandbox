using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.events
{
    public class SleepChangedEventArgs : EventArgs
    {
        public int AddedSleepValue { get; }
        public int CurrentBeingSleep { get; }
        public SleepChangedEventArgs(int addedSleepValue,int currentBeingSleep) 
        {
            AddedSleepValue = addedSleepValue;
            CurrentBeingSleep = currentBeingSleep;
        }
    }
}
