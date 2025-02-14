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

        private ITask? _waitFor; // فیلد پشتیبان برای WaitFor
        public ITask? Waited { get; set; }
        public event Action<ITask>? OnCompleted;

        public ITask? WaitFor
        {
            get => _waitFor;
            set
            {
                _waitFor = value;
                IsWaited = _waitFor != null; // به‌روزرسانی IsWaited هنگام تغییر WaitFor
            }
        }

        public bool IsWaited { get; private set; } // فقط‌خوان و به‌صورت خودکار به‌روزرسانی می‌شود

        public void ExecuteStep(LivingBeing being, World world)
        {
            //form.WriteLine($"🍼 {being.Name} تولیدمثل کرد!");
            LivingBeing human = new LivingBeing("فرزند_" + being.Name, being.Location);
            human.Tasks.Add(new MoveTask(new Location(being.Location.X + 20, being.Location.Y + 20)));
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
