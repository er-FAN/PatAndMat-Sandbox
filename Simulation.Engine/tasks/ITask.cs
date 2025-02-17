using Simulation.Engine.events;
using Simulation.Engine.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.tasks
{
    public interface ITask
    {

        string Name { get; }
        bool IsCompleted { get; }
        public ITask? WaitFor { get; set; }
        public ITask? Waited { get; set; }
        bool IsWaited { get; }
        void ExecuteStep(LivingBeing being, World world);
        event EventHandler<TaskCompletedEventArgs> OnCompleted;
        void TaskCompleted(object? sender, TaskCompletedEventArgs e);
        void WaitForCompleted(object? sender, TaskCompletedEventArgs e);
        void ForceStop();
    }
}
