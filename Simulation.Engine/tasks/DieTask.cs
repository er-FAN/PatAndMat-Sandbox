//using Simulation.Engine.events;
//using Simulation.Engine.models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Simulation.Engine.tasks
//{
//    public class DieTask : ITask
//    {
//        public string Name => "مردن";

//        public bool IsCompleted { get; private set; } = false;
//        private ITask? _waitFor; // فیلد پشتیبان برای WaitFor
//        public ITask? Waited { get; set; }
//        public event EventHandler<TaskCompletedEventArgs> OnCompleted;

//        public ITask? WaitFor
//        {
//            get => _waitFor;
//            set
//            {
//                _waitFor = value;
//                IsWaited = _waitFor != null; // به‌روزرسانی IsWaited هنگام تغییر WaitFor
//                if (WaitFor != null)
//                {
//                    WaitFor.OnCompleted += WaitFor_OnCompleted;
//                }
//            }
//        }

//        public bool IsWaited { get; private set; } // فقط‌خوان و به‌صورت خودکار به‌روزرسانی می‌شود

//        public LivingBeing Executer { get; }

//        public DieTask(LivingBeing executer)
//        {
//            Executer = executer;
//            OnCompleted += Task_OnCompleted;
//        }

//        public void ExecuteStep(World world)
//        {
//            Die(world);
//            OnCompleted.Invoke(this, new TaskCompletedEventArgs());
//        }

//        private void Die(World world)
//        {
//            Executer.IsAlive = false;
//            MoveDiedEntityToDiedEntitiesList(world);
//        }

//        private void MoveDiedEntityToDiedEntitiesList(World world)
//        {
//            world.DiedEntities.Add(Executer);
//            world.Entities.Remove(Executer);
//        }

//        public void ForceStop() { }

//        public void Task_OnCompleted(object? sender, TaskCompletedEventArgs e)
//        {
//            IsCompleted = true;
//        }

//        public void WaitFor_OnCompleted(object? sender, TaskCompletedEventArgs e)
//        {
//            IsWaited = false;
//        }

//    }
//}
