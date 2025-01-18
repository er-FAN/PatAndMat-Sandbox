using Simulation.Engine.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.tasks
{
    public class RestTask : ITask
    {
        //Form1 form = new Form1();
        public string Name => "استراحت";
        public bool IsCompleted { get; private set; } = false;
        int steps = 0;
        public void ExecuteStep(LivingBeing being, World world)
        {
            //form.WriteLine($"💤 {being.Name} در حال استراحت است.");
            being.Sleep -= 4;
            if (being.Sleep <= 0)
            {
                IsCompleted = true;
            }
        }



        public void ForceStop()
        {
            throw new NotImplementedException();
        }
    }
}
