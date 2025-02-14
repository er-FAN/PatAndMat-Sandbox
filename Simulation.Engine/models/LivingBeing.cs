using Simulation.Engine.tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Simulation.Engine.models
{
    public class LivingBeing : PhysicalObject
    {
        public string Name { get; set; }
        public bool Gender { get; set; }
        public int Age { get; set; }
        public int VisualRange { get; set; } = 500;
        public bool IsAlive { get; set; }
        public int Energy { get; set; }
        public int Sleep { get; set; }
        public bool IsSleep { get; set; }
        public List<ITask> Tasks { get; set; } = new List<ITask>();
        public List<EdibleObject> EdibleObjects { get; set; } = new List<EdibleObject>();
        public EventManager EventManager { get; set; } = new EventManager();

        public LivingBeing(string name, Location location)
        {
            Guid id = Guid.NewGuid();
            Name = name;
            Age = 0;
            IsAlive = true;
            Energy = 100;
            Sleep = 0;
            Location = location;

            EventManager.RegisterEvent("Hungry", () => Tasks.Add(new EatTask()));
            EventManager.RegisterEvent("Tired", () => Tasks.Add(new RestTask()));
            EventManager.RegisterEvent("Reproduce", () => Tasks.Add(new ReproduceTask()));
            EventManager.RegisterEvent("Die", () => Tasks.Add(new Die()));
        }

        public void Update(World world)
        {
            foreach (var entity in world.NewEntities)
            {
                if (entity == null)
                {

                }
            }


            Age++;
            Sleep++;
            Energy -= IsSleep ? 1 : 2;


            CheckConditions(world);

            if (!IsAlive)
            {
                return;
            }

            if (Tasks.Count > 0)
            {
                world.Output.Entities.Add(this);
                for (int i = Tasks.Count - 1; i >= 0; i--)
                {
                    ITask task = Tasks[i];

                    task.ExecuteStep(this, world);

                    if (task.IsCompleted)
                    {
                        //form.WriteLine($"✅ تسک '{task.Name}' برای {Name} به پایان رسید.");
                        Tasks.RemoveAt(i);
                    }
                }
            }


        }

        private void CheckConditions(World world)
        {
            //if (Sleep > 20) EventManager.TriggerEvent("Tired");
            if (!IsSleep)
            {
                if (Energy < 30) EventManager.TriggerEvent("Hungry");
                //if (Age > 20 && Age % 50 == 0) EventManager.TriggerEvent("Reproduce");
            }
            if (Age > 10000000 || Energy == 0)
            {
                //EventManager.TriggerEvent("Die");
            }

        }

        public override string ToString()
        {
            return $"{Name} | سن: {Age} | انرژی: {Energy} | مکان: {Location} | زنده: {IsAlive}";
        }
    }
}
