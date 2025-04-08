//using Simulation.Engine.events;
//using Simulation.Engine.models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Simulation.Engine.tasks
//{
//    public class ReproduceTask : ITask
//    {
//        public string Name => "تولیدمثل";
//        public LivingBeing Executer { get; }
//        public bool IsCompleted { get; private set; } = false;

//        private ITask? _waitFor;

//        public event EventHandler<TaskCompletedEventArgs> OnCompleted = delegate { };

//        public ITask? Waited { get; set; }


//        public ITask? WaitFor
//        {
//            get => _waitFor;
//            set
//            {
//                _waitFor = value;
//                IsWaited = _waitFor != null;
//                if (WaitFor != null)
//                {
//                    WaitFor.OnCompleted += WaitFor_OnCompleted;
//                }
                
//            }
//        }

//        public bool IsWaited { get; private set; }

//        public ReproduceTask(LivingBeing executer)
//        {
//            Executer = executer;
//            OnCompleted += Task_OnCompleted;
//        }
//        public void ExecuteStep(World world)
//        {
//            LivingBeing human = new LivingBeing("فرزند_" + Executer.Name, Executer.Location);
//            world.Entities.Add(human);
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
