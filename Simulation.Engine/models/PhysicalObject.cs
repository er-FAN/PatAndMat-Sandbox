using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Simulation.Engine.models
{
    public class PhysicalObject
    {
        public Guid Id { get; set; }
        public Type Type { get; set; } = typeof(PhysicalObject);
        public PhysicalObject? Parent { get; set; }
        public Location Location { get; set; } = new Location(0, 0);
        public int Width { get; set; }  // X: عرض
        public int Height { get; set; } // Y: ارتفاع

        //محدوده فاصله
        public int Lenght2 { get; set; }
        public int Width2 { get; set; }

        public PhysicalObject()
        {
            Id = Guid.NewGuid();
        }




    }
}
