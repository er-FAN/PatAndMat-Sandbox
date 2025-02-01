using Simulation.Engine.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.tasks
{
    public class Die : ITask
    {
        public string Name => "مردن";

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
            being.IsAlive = false;
            world.DiedEntities.Add(being);
            world.Entities.Remove(being);
            IsCompleted = true;
        }

        public void ForceStop() { }
    }
}
