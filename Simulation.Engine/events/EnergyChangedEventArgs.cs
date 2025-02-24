using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.events
{
    public class EnergyChangedEventArgs : EventArgs
    {
        public int AddedEnergyValue { get; }
        public int CurrentBeingEnergy { get; }
        public EnergyChangedEventArgs(int addedEnergyValue, int currentBeingEnergy)
        {
            AddedEnergyValue = addedEnergyValue;
            CurrentBeingEnergy = currentBeingEnergy;
        }
    }
}
