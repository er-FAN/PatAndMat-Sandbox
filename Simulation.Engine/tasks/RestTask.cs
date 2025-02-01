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
        private ITask? _waitFor; // فیلد پشتیبان برای WaitFor
        public ITask? Waited { get; set; }
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

        int steps = 0;

        public event Action<ITask>? OnCompleted;

        public void ExecuteStep(LivingBeing being, World world)
        {
            //form.WriteLine($"💤 {being.Name} در حال استراحت است.");
            steps++;
            being.Sleep -= 4;
            if (being.Sleep <= 0)
            {
                IsCompleted = true;
                steps = 0;
            }
        }



        public void ForceStop()
        {
            throw new NotImplementedException();
        }
    }
}
