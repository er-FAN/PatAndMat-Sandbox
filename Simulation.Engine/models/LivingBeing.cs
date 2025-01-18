using Simulation.Engine.tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Simulation.Engine.models
{
    public class LivingBeing
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsAlive { get; set; }
        public int Energy { get; set; }
        public int Sleep { get; set; }
        public bool IsSleep { get; set; }
        public Location Location { get; set; }
        public List<ITask> Tasks { get; set; } = new List<ITask>();
        public EventManager EventManager { get; set; } = new EventManager();
        //Form1 form = new Form1();
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
            EventManager.RegisterEvent("Reproduce", () => Tasks.Add(new ReproduceTask(EventManager.SimulationEntities)));
        }

        public void Update(World world)
        {
            //world.Entities.AddRange(world.NewEntities);
            foreach (var entity in world.NewEntities)
            {
                if (entity == null)
                {

                }
            }
            //world.NewEntities.Clear();
            if (!IsAlive) return;

            Age++;
            Sleep++;
            Energy -= IsSleep ? 1 : 2;

            if (Energy <= 0)
            {
                Marg(world);
                //form.WriteLine($"💀 {Name} از بین رفت (انرژی تمام شد).");
                return;
            }

            CheckConditions(world);
            if (Tasks.Count > 0)
            {
                world.Output.Entities.Add(this);
                for (int i = Tasks.Count - 1; i >= 0; i--)
                {
                    var task = Tasks[i];
                    task.ExecuteStep(this, world);
                    if (task.IsCompleted)
                    {
                        //form.WriteLine($"✅ تسک '{task.Name}' برای {Name} به پایان رسید.");
                        Tasks.RemoveAt(i);
                    }
                }
            }


        }

        private void Marg(World world)
        {
            IsAlive = false;
            world.DiedEntities.Add(this);
            world.Entities.Remove(this);
        }

        private void CheckConditions(World world)
        {
            if (Sleep > 20) EventManager.TriggerEvent("Tired");
            if (!IsSleep)
            {
                if (Energy < 30) EventManager.TriggerEvent("Hungry");
                if (Age > 20 && Age % 50 == 0) EventManager.TriggerEvent("Reproduce");
            }
            if (Age > 100)
            {
                Marg(world);
            }

        }

        public override string ToString()
        {
            return $"{Name} | سن: {Age} | انرژی: {Energy} | مکان: {Location} | زنده: {IsAlive}";
        }
    }
}
