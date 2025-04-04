using Simulation.Engine.models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.events
{
    public class LocationChangedEventArgs : EventArgs, IConstructible<Location>
    {
        public Location OldLocation { get; private set; }
        public Location NewLocation { get; private set; }

        public LocationChangedEventArgs(Location oldLocation, Location newLocation)
        {
            OldLocation = oldLocation;
            NewLocation = newLocation;
        }

        public LocationChangedEventArgs()
        {
            
        }

        public void Construct(Location param1, Location param2)
        {
            OldLocation = param1;
            NewLocation = param2;
        }
    }
}
