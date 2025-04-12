using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.App.Models
{
    public class Node
    {
        public double X { get; set; }
        public double Y { get; set; }

        public bool Equals(Node node)
        {
            return this.X == node.X && this.Y == node.Y;
        }
    }
}
