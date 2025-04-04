using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.events
{
    public class EnergyChangedEventArgs : EventArgs, IConstructible<int>
    {
        public int AddedEnergyValue { get; private set; }
        public int CurrentBeingEnergy { get; private set; }
        
        public EnergyChangedEventArgs()
        {
            
        }

        public void Construct(int param1, int param2)
        {
            AddedEnergyValue = param1;
            CurrentBeingEnergy = param2;
        }
    }
}
