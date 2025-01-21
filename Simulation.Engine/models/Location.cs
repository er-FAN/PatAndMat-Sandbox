using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.models
{
    public class Location(int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
