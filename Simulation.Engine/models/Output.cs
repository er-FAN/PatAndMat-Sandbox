using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.models
{
    public class Output
    {
        public int FoodSupply { get; set; }
        public int WaterSupply { get; set; }
        public List<LivingBeing> Entities { get; set; } = [];
        public List<LivingBeing> DiedEntities { get; set; } = [];
        public List<LivingBeing> NewEntities { get; set; } = [];

        DataTable Table { get; set; } = new DataTable();

        //public Output GetOutput()
        //{

        //}
    }
}
