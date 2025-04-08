//using Simulation.Engine.events;
//using Simulation.Engine.models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Permissions;
//using System.Text;
//using System.Threading.Tasks;

//namespace Simulation.Engine.tasks
//{
//    public class RestTask : ITask
//    {
//        //Form1 form = new Form1();
//        public string Name => "استراحت";
//        public LivingBeing Executer { get; }
//        public bool IsCompleted { get; private set; } = false;
//        private ITask? _waitFor; // فیلد پشتیبان برای WaitFor
//        public ITask? Waited { get; set; }
//        public ITask? WaitFor
//        {
//            get => _waitFor;
//            set
//            {
//                _waitFor = value;
//                IsWaited = _waitFor != null; // به‌روزرسانی IsWaited هنگام تغییر WaitFor
//            }
//        }

//        public bool IsWaited { get; private set; } // فقط‌خوان و به‌صورت خودکار به‌روزرسانی می‌شود

//        public event EventHandler<TaskCompletedEventArgs> OnCompleted = delegate { };

//        public RestTask(LivingBeing executer)
//        {
//            Executer = executer;
//            RegisterEvents();
//        }

//        private void RegisterEvents()
//        {
//            OnCompleted += Task_OnCompleted;
//            Executer.SleepChanged += Executer_SleepChanged;
//        }

//        private void Executer_SleepChanged(object? sender, SleepChangedEventArgs e)
//        {
//            if (e.CurrentBeingSleep < 30)
//            {
//                OnCompleted.Invoke(this, new TaskCompletedEventArgs());
//            }
//        }

//        public void ExecuteStep(World world)
//        {
//            Executer.Sleep.Value -= 4;
//        }



//        public void ForceStop()
//        {
//            throw new NotImplementedException();
//        }

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
