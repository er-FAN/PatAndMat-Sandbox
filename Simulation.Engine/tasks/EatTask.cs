using Simulation.Engine.events;
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
        public string Name => "خوردن غذا";
        public bool IsCompleted { get; private set; } = false;
        private int steps = 0;
        private ITask? _waitFor;

        public event EventHandler<TaskCompletedEventArgs> OnCompleted;

        public ITask? WaitFor
        {
            get => _waitFor;
            set
            {
                _waitFor = value;
                IsWaited = _waitFor != null;
                if (WaitFor != null)
                {
                    WaitFor.OnCompleted += WaitForCompleted;
                }
            }
        }
        public ITask? Waited { get; set; }
        public bool IsWaited { get; private set; }
        EdibleObject EdibleObject { get; set; } = new EdibleObject();

        public EatTask()
        {
            OnCompleted +=WaitForCompleted;
        }

        public void ExecuteStep(LivingBeing being, World world)
        {
            steps++;
            if (being.EdibleObjects.Count > 0)
            {
                EdibleObject = being.EdibleObjects.First();
                if (EdibleObject != null)
                {
                    if (EdibleObject.Energy > 5)
                    {

                        being.Energy += 5;
                        EdibleObject.Energy -= 5;
                    }
                    else if (EdibleObject.Energy > 0)
                    {
                        being.Energy += EdibleObject.Energy;
                        being.EdibleObjects.Remove(EdibleObject);
                    }
                }
                else
                {
                    SearchTask searchTask = new SearchTask(new EdibleObject());
                    searchTask.Waited = this;
                    WaitFor = searchTask;
                    being.Tasks.Add(searchTask);
                }


            }
            else
            {
                SearchTask searchTask = new SearchTask(new EdibleObject());
                searchTask.Waited = this;
                WaitFor = searchTask;
                being.Tasks.Add(searchTask);
            }


            if (steps >= 3)
            {
                OnCompleted.Invoke(this, new TaskCompletedEventArgs());
            }
        }

        public void ForceStop()
        {
            throw new NotImplementedException();
        }

        public void TaskCompleted(object? sender, TaskCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void WaitForCompleted(object? sender, TaskCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
