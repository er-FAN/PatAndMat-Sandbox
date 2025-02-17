using Simulation.Engine.models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.events
{
    public class LocationChangedEventArgs : EventArgs
    {
        public Location OldLocation { get; }
        public Location NewLocation { get; }

        public LocationChangedEventArgs(Location oldLocation, Location newLocation)
        {
            OldLocation = oldLocation;
            NewLocation = newLocation;
        }
    }
}
