using Simulation.Engine.events;
using Simulation.Engine.models;

namespace Simulation.Engine.tasks
{
    public class MoveTask : ITask
    {
        public string Name => "حرکت";
        public LivingBeing Executer { get; }
        public float SpeedFactor { get; set; }
        public bool IsCompleted { get; private set; } = false;
        public Location Destination;
        private ITask? _waitFor;

        public event EventHandler<TaskCompletedEventArgs> OnCompleted;

        public ITask? Waited { get; set; }


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

        public bool IsWaited { get; private set; }

        public MoveTask(LivingBeing executer, Location destination)
        {
            Executer = executer;
            Destination = destination;
            OnCompleted += Task_OnCompleted;
            Executer.EnergyChanged += Executer_EnergyChanged;
        }

        private void Executer_EnergyChanged(object? sender, EnergyChangedEventArgs e)
        {
            if (e.CurrentBeingEnergy < 50)
            {
                SpeedFactor = e.CurrentBeingEnergy / 50;
            }
        }

        public void ExecuteStep(World world)
        {
            Move();
            bool ArrivedDestination = Executer.Location.X == Destination.X && Executer.Location.Y == Destination.Y;
            if (ArrivedDestination)
            {
                IsCompleted = true;
                OnCompleted?.Invoke(this, new TaskCompletedEventArgs());
            }
        }

        private void Move()
        {
            MoveInX();
            MoveInY();
        }

        private void MoveInY()
        {
            if (Executer.Location.Y < Destination.Y) Executer.Location.Y++;
            else if (Executer.Location.Y > Destination.Y) Executer.Location.Y--;
        }

        private void MoveInX()
        {
            if (Executer.Location.X < Destination.X) Executer.Location.X++;
            else if (Executer.Location.X > Destination.X) Executer.Location.X--;
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
            IsWaited = false;
        }
    }
}
