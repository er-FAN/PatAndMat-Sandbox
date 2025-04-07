using Simulation.Engine.events;
using Simulation.Engine.models;

namespace Simulation.Engine.tasks
{

    public class EatTask : ITask
    {
        public string Name => "خوردن غذا";
        public bool IsCompleted { get; private set; } = false;
        private int steps = 0;
        public event EventHandler<TaskCompletedEventArgs> OnCompleted = delegate { };

        private ITask? _waitFor;
        public ITask? WaitFor
        {
            get => _waitFor;
            set
            {
                _waitFor = value;
                IsWaited = _waitFor != null;
                if (WaitFor != null)
                {
                    WaitFor.OnCompleted += WaitFor_OnCompleted;
                }
            }
        }
        public ITask? Waited { get; set; }
        public bool IsWaited { get; private set; }
        EdibleObject EdibleObject { get; set; } = new EdibleObject();

        public LivingBeing Executer { get; }

        public EatTask(LivingBeing executer)
        {
            Executer = executer;
            RegisterEvents(executer);
        }

        private void RegisterEvents(LivingBeing executer)
        {
            OnCompleted += WaitFor_OnCompleted;
            executer.EnergyChanged += Being_EnergyChanged;
        }

        public void ExecuteStep(World world)
        {
            bool haveFood = Executer.EdibleObjects.Count > 0 && Executer.EdibleObjects.First() != null;
            if (haveFood)
            {
                EdibleObject = Executer.EdibleObjects.First();
                AddEnergyFromEdibleObjectToBeing();
            }
            else
            {
                AddSearchTaskForFindEdibleObject();
            }
        }

        private void AddEnergyFromEdibleObjectToBeing()
        {
            if (EdibleObject.Energy > 5)
            {
                Eat();
            }
            else if (EdibleObject.Energy > 0)
            {
                EatAndRemoveFinishedEdibleObject();
            }

        }

        private void EatAndRemoveFinishedEdibleObject()
        {
            Executer.Energy.Value += EdibleObject.Energy;
            Executer.EdibleObjects.Remove(EdibleObject);
        }

        private void Eat()
        {
            Executer.Energy.Value += 5;
            EdibleObject.Energy -= 5;
        }

        private void AddSearchTaskForFindEdibleObject()
        {
            SearchTask searchTask = new SearchTask(Executer, new EdibleObject());
            searchTask.Waited = this;
            WaitFor = searchTask;
            Executer.Tasks.Add(searchTask);
        }

        public void ForceStop()
        {
            throw new NotImplementedException();
        }

        public void Task_OnCompleted(object? sender, TaskCompletedEventArgs e)
        {
            IsCompleted = true;
        }

        public void WaitFor_OnCompleted(object? sender, TaskCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Being_EnergyChanged(object? sender, EnergyChangedEventArgs e)
        {
            if (e.CurrentBeingEnergy >= 70)
            {
                OnCompleted.Invoke(this, new TaskCompletedEventArgs());
            }
        }
    }
}
