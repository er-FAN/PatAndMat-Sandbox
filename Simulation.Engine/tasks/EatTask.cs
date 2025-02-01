using Simulation.Engine.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.tasks
{

    public class EatTask : ITask
    {
        //Form1 form = new Form1();
        public string Name => "خوردن غذا";
        public bool IsCompleted { get; private set; } = false;
        private int steps = 0;
        private ITask? _waitFor; // فیلد پشتیبان برای WaitFor

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
        public ITask? Waited { get; set; }
        public bool IsWaited { get; private set; } // فقط‌خوان و به‌صورت خودکار به‌روزرسانی می‌شود

        public void ExecuteStep(LivingBeing being, World world)
        {

            //form.WriteLine($"🍽️ {being.Name} در حال خوردن غذا (مرحله {steps}).");

            if (world.FoodSupply > 0)
            {
                steps++;
                being.Energy += 5;
                world.FoodSupply -= 5;
            }
            else
            {
                steps = 3;
            }


            if (steps >= 3)
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
