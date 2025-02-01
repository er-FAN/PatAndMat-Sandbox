using Simulation.Engine.tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.models
{
    public class LivingBeingTaskHistory
    {
        public LivingBeing LivingBeing { get; set; }
        public SearchTask SearchTask { get; set; }
        public MoveTask MoveTask { get; set; }
        public RestTask RestTask { get; set; }

    }
}
