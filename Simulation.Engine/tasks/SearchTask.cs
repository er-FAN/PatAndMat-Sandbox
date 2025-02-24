using Simulation.Engine.events;
using Simulation.Engine.models;

namespace Simulation.Engine.tasks
{
    public class SearchTask : ITask
    {
        public string Name => "جستجو";
        public LivingBeing Executer { get; }
        public PhysicalObject SearchFor { get; }
        public bool IsCompleted { get; private set; } = false;
        private ITask? _waitFor; // فیلد پشتیبان برای WaitFor

        public ITask? WaitFor
        {
            get => _waitFor;
            set
            {
                _waitFor = value;
                IsWaited = _waitFor != null; // به‌روزرسانی IsWaited هنگام تغییر WaitFor
                if (WaitFor != null)
                {
                    WaitFor.OnCompleted += WaitFor_OnCompleted;
                }

            }
        }

        public bool IsWaited { get; private set; } // فقط‌خوان و به‌صورت خودکار به‌روزرسانی می‌شود
        public ITask? Waited { get; set; }

        public List<PhysicalObject> objects = [];
        public List<PhysicalObject> foundObjects = [];
        MoveTask moveTask;

        public event EventHandler<TaskCompletedEventArgs> OnCompleted;

        public SearchTask(LivingBeing executer,PhysicalObject searchFor)
        {
            SearchFor = searchFor;
            Executer = executer;
            moveTask = new MoveTask(Executer,new Location(0, 0));
            OnCompleted += Task_OnCompleted;

        }

        public void ExecuteStep(World world)
        {
            FilterPhysicalObjectsBySearchObjectType(world);

            // پیدا کردن اشیاء در محدوده دید
            foundObjects = objects.Where(obj => IsInVisualRange(obj)).ToList();

            if (foundObjects.Any())
            {
                MoveToFoundedObjectLocation();
                foreach (PhysicalObject obj in foundObjects)
                {
                    EdibleObject edibleObject = world.EdibleObjects.Where(x => x.Id == obj.Id).FirstOrDefault();
                    world.EdibleObjects.Remove(edibleObject);
                    Executer.EdibleObjects.Add(edibleObject);
                }
                IsCompleted = true; // اگر اشیائی پیدا شدند، تسک کامل می‌شود
                TaskCompletedEventArgs e = new TaskCompletedEventArgs();
                OnCompleted.Invoke(this, e);
                return;
            }

            MoveToRandomLocation(world);

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

        private void MoveToFoundedObjectLocation()
        {
            moveTask.Destination = foundObjects.First().Location;
            Executer.Tasks.Add(moveTask);
        }

        private void MoveToRandomLocation(World world)
        {
            AddMoveTask(world);
            WaitForMoveTaskEnd();
        }

        private void AddMoveTask(World world)
        {
            Location location = new(0, 0);
            location = GetRandomLocation(world);
            moveTask.Destination = location;
            Executer.Tasks.Add(moveTask);
        }

        private void WaitForMoveTaskEnd()
        {
            moveTask.Waited = this;
            WaitFor = moveTask;
        }

        private Location GetRandomLocation(World world)
        {
            Location location;
            int newX = RandomMoveOffset(-25, 25);
            int newY = RandomMoveOffset(-25, 25);
            location = new Location(Executer.Location.X + newX, Executer.Location.Y + newY);
            location = CheckIsInCorners(location, world.Width, world.Height);
            return location;
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

        private bool IsInVisualRange(PhysicalObject obj)
        {
            // فاصله بین موجود زنده و جسم هدف
            int deltaX = Math.Abs(Executer.Location.X - obj.Location.X);
            int deltaY = Math.Abs(Executer.Location.Y - obj.Location.Y);

            // بررسی اینکه جسم در محدوده دید است یا نه
            return deltaX <= Executer.VisualRange && deltaY <= Executer.VisualRange;
        }

        private int RandomMoveOffset(params int[] numbers)
        {
            Random random = new Random();
            if (numbers.Length == 0)
                throw new ArgumentException("باید حداقل یک عدد وارد کنید!");

            int index = random.Next(numbers.Length);  // انتخاب یک ایندکس تصادفی
            return numbers[index];  // بازگرداندن مقدار انتخاب‌شده
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

        public void Task_OnCompleted(object? sender, TaskCompletedEventArgs e)
        {
            IsCompleted = true;
        }

        public void WaitFor_OnCompleted(object? sender, TaskCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }





}
