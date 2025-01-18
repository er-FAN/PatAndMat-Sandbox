using Simulation.Engine.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.tasks
{
    public class ReproduceTask : ITask
    {
        //Form1 form=new Form1();
        public string Name => "تولیدمثل";
        public bool IsCompleted { get; private set; } = false;
        private List<LivingBeing> entities;

        public ReproduceTask(List<LivingBeing> entities)
        {
            this.entities = entities;
        }

        public void ExecuteStep(LivingBeing being, World world)
        {
            //form.WriteLine($"🍼 {being.Name} تولیدمثل کرد!");
            LivingBeing human = new LivingBeing("فرزند_" + being.Name, new Location(0, 0));
            world.Entities.Add(human);
            world.Output.NewEntities.Add(human);
            IsCompleted = true;
        }

        public void ForceStop()
        {
            throw new NotImplementedException();
        }
    }
}
