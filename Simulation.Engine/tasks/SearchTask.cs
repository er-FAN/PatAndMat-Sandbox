using Simulation.Engine.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.tasks
{
    public class SearchTask : ITask
    {
        public string Name => "جستجو";
        public PhysicalObject searchFor;
        public bool IsCompleted { get; private set; } = false;
        private ITask? _waitFor; // فیلد پشتیبان برای WaitFor

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
        public ITask? Waited { get; set; }

        public List<PhysicalObject> objects = [];
        public List<PhysicalObject> foundObjects = [];
        MoveTask moveTask;

        public event Action<ITask>? OnCompleted;

        public SearchTask(PhysicalObject searchFor)
        {
            this.searchFor = searchFor;
            moveTask = new MoveTask(new Location(0, 0));
        }

        public void ExecuteStep(LivingBeing being, World world)
        {
            FilterPhysicalObjectsBySearchObjectType(world);

            // پیدا کردن اشیاء در محدوده دید
            foundObjects = objects.Where(obj => IsInVisualRange(being, obj)).ToList();

            if (foundObjects.Any())
            {
                MoveToFoundedObjectLocation(being);
                IsCompleted = true; // اگر اشیائی پیدا شدند، تسک کامل می‌شود
                OnCompleted?.Invoke(this);
                return;
            }

            MoveToRandomLocation(being, world);

        }

        private void FilterPhysicalObjectsBySearchObjectType(World world)
        {
            //if (searchFor.Type == typeof(EdibleObject))
            //{
            //    objects.AddRange(world.EdibleObjects);
            //}
            //if (searchFor.Type == typeof(LivingBeing))
            //{SSS
            //    objects.AddRange(world.Entities);
            //}

            objects.AddRange(world.EdibleObjects);
        }

        private void MoveToFoundedObjectLocation(LivingBeing being)
        {
            moveTask.Destination = foundObjects.First().Location;
            being.Tasks.Add(moveTask);
        }

        private void MoveToRandomLocation(LivingBeing being, World world)
        {
            Location location = new(0, 0);

            int newX = RandomMoveOffset();
            int newY = RandomMoveOffset();
            location = new Location(being.Location.X + newX, being.Location.Y + newY);
            location = CheckIsInCorners(location, world.Width, world.Height);


            moveTask.Destination = location;
            // اگر چیزی پیدا نشد، حرکت کنید
            being.Tasks.Add(moveTask);
            moveTask.Waited = this;
            WaitFor = moveTask;
        }

        private Location CheckIsInCorners(Location location, int width, int height)
        {
            if (location.X > width || location.X < 0)
            {
                location.X *= -1;
            }
            if (location.Y > height || location.Y < 0)
            {
                location.Y *= -1;
            }
            return location;
        }

        private bool IsInVisualRange(LivingBeing being, PhysicalObject obj)
        {
            // فاصله بین موجود زنده و جسم هدف
            int deltaX = Math.Abs(being.Location.X - obj.Location.X);
            int deltaY = Math.Abs(being.Location.Y - obj.Location.Y);

            // بررسی اینکه جسم در محدوده دید است یا نه
            return deltaX <= being.VisualRange && deltaY <= being.VisualRange;
        }

        private int RandomMoveOffset()
        {
            Random random = new Random();
            return random.Next(-25, 25); // مقدار تصادفی بین -1 و 1
        }

        public void ForceStop()
        {
            throw new NotImplementedException();
        }

        private void HandleWaitForCompleted(ITask completedTask)
        {
            Console.WriteLine($"تسک {Name} دیگر منتظر {completedTask.Name} نیست!");
            WaitFor = null; // خالی کردن پراپرتی WaitFor      // اجرای تسک بعدی
        }
    }
}
