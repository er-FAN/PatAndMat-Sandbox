using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.models
{
    //class World
    //{
    //    public string Name { get; set; }
    //    public int Width { get; set; }
    //    public int Height { get; set; }
    //    public int FoodSupply { get; set; }
    //    public int WaterSupply { get; set; }
    //    public List<LivingBeing> Entities { get; set; }
    //    public List<LivingBeing> NewEntities { get; set; }
    //    public List<LivingBeing> DiedEntities { get; set; }

    //    public EventManager EventManager { get; set; } = new EventManager();


    //    public World(string name, int width, int height, int initialFood, int initialWater)
    //    {
    //        Name = name;
    //        Width = width;
    //        Height = height;
    //        FoodSupply = initialFood;
    //        WaterSupply = initialWater;
    //        Entities = new List<LivingBeing>();
    //        NewEntities = new List<LivingBeing>();
    //        DiedEntities = new List<LivingBeing>();

    //        EventManager.RegisterEvent("SeasonChange", OnSeasonChange);
    //        EventManager.RegisterEvent("Earthquake", OnEarthquake);
    //    }

    //    public void AddEntity(LivingBeing entity)
    //    {
    //        // اطمینان از اینکه موجود در محدوده زمین قرار دارد
    //        if (entity.Location.X < 0 || entity.Location.X >= Width || entity.Location.Y < 0 || entity.Location.Y >= Height)
    //        {
    //            throw new ArgumentException("مکان موجود زنده خارج از محدوده زمین است!");
    //        }

    //        Entities.Add(entity);
    //        EventManager.SimulationEntities.Add(entity);
    //    }

    //    public void Update()
    //    {
    //        //form.WriteLine($"\n🌍 وضعیت زمین '{Name}' | غذا: {FoodSupply} | آب: {WaterSupply}");
    //        var entitiesCopy = Entities.ToList();
    //        foreach (var entity in entitiesCopy)
    //        {
    //            entity.Update(this);
    //        }

    //        Entities.AddRange(NewEntities);
    //        NewEntities = [];

    //        if (new Random().Next(0, 10) < 2)
    //        {
    //            EventManager.TriggerEvent("SeasonChange");
    //        }

    //        if (new Random().Next(0, 50) == 1)
    //        {
    //            EventManager.TriggerEvent("Earthquake");
    //        }
    //    }

    //    private void OnSeasonChange()
    //    {
    //        //form.WriteLine("🍂 تغییر فصل رخ داد! منابع غذا و آب تغییر یافتند.");
    //        FoodSupply += 50;
    //        WaterSupply += 30;
    //    }

    //    private void OnEarthquake()
    //    {
    //        //form.WriteLine("🌋 زلزله رخ داد! منابع کاهش یافتند و برخی موجودات آسیب دیدند.");
    //        FoodSupply = Math.Max(FoodSupply - 20, 0);
    //        WaterSupply = Math.Max(WaterSupply - 10, 0);
    //    }
    //}


















    public class World : PhysicalObject
    {
        public string Name { get; set; } = string.Empty;
        public int FoodSupply { get; set; }
        public int WaterSupply { get; set; }
        public List<LivingBeing> Entities { get; set; } = [];
        public List<LivingBeing> DiedEntities { get; set; } = [];
        public List<LivingBeing> NewEntities { get; set; } = [];
        public List<Location> FilledLocations { get; set; } = [];
        public List<Location> EmptyLocations { get; set; } = [];
        public List<EdibleObject> EdibleObjects { get; set; } = [];
        public List<Lake> Lakes { get; set; } = [];

        public EventManager EventManager { get; set; } = new EventManager();

        public Output Output { get; set; } = new Output();

        public World(string name, int width, int height, int initialFood, int initialWater)
        {
            Name = name;
            Width = width;
            Height = height;
            FoodSupply = initialFood;
            WaterSupply = initialWater;

            WaterDistribution();
            FoodDistribution(500, 5, 50);


            EventManager.RegisterEvent("SeasonChange", OnSeasonChange);
            EventManager.RegisterEvent("Earthquake", OnEarthquake);
        }

        public void WaterDistribution()
        {
            Lakes.Add(new Lake(450, 200, 300, 250));
        }

        public void FoodDistribution(int count, int minEnergy, int maxEnergy)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than zero.", nameof(count));

            if (minEnergy <= 0 || maxEnergy <= 0 || minEnergy > maxEnergy)
                throw new ArgumentException("Energy range is invalid.");

            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                // ایجاد مکان رندوم برای خوراکی
                int x = random.Next(0, Width);
                int y = random.Next(0, Height);
                var location = new Location(x, y);

                // بررسی اینکه مکان خالی است
                bool isOccupied = Entities.Any(entity =>
                {
                    var entityBounds = new Rectangle(entity.Location.X, entity.Location.Y, entity.Width, entity.Height);
                    var foodBounds = new Rectangle(location.X, location.Y, 1, 1); // فرض کنیم هر خوراکی اندازه 1x1 دارد
                    return entityBounds.IntersectsWith(foodBounds);
                });

                if (isOccupied || IsPointInsideLake(new Point(location.X, location.Y), Lakes[0]))
                {
                    i--; // اگر مکان اشغال شده، دوباره امتحان می‌کنیم
                    continue;
                }

                // ایجاد خوراکی با مقدار انرژی رندوم
                int energy = random.Next(minEnergy, maxEnergy + 1);

                var food = new EdibleObject
                {
                    Type = typeof(EdibleObject),
                    Height = 1,
                    Width = 1,
                    Location = location,
                    Energy = energy
                };

                // اضافه کردن خوراکی به موجودیت‌های جهان
                EdibleObjects.Add(food);
            }
        }

        public bool IsPointInsideLake(PointF point, Lake lake)
        {
            int intersections = 0;
            List<PointF> points = lake.Points;
            int count = points.Count;

            for (int i = 0; i < count; i++)
            {
                PointF p1 = points[i];
                PointF p2 = points[(i + 1) % count]; // نقطه بعدی (مدول برای بسته بودن چندضلعی)

                // بررسی اینکه آیا خط افقی از نقطه داده شده، با ضلع چندضلعی تلاقی دارد
                if ((point.Y > Math.Min(p1.Y, p2.Y)) &&
                    (point.Y <= Math.Max(p1.Y, p2.Y)) &&
                    (point.X <= Math.Max(p1.X, p2.X)))
                {
                    // محاسبه موقعیت تلاقی
                    float xIntersection = p1.X + (point.Y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y);
                    if (p1.Y != p2.Y && point.X < xIntersection)
                    {
                        intersections++;
                    }
                }
            }

            // اگر تعداد تلاقی‌ها فرد باشد، نقطه داخل است
            return (intersections % 2) != 0;
        }



        public void AddEntity(LivingBeing entity)
        {
            if (entity.Location.X < 0 || entity.Location.X >= Width || entity.Location.Y < 0 || entity.Location.Y >= Height)
            {
                throw new ArgumentException("مکان موجود زنده خارج از محدوده زمین است!");
            }

            Entities.Add(entity);
        }

        public async Task UpdateAsync()
        {
            var entitiesCopy = Entities.ToList();

            if (Entities == null)
            {
                throw new InvalidOperationException("Entities list is not initialized!");
            }

            int maxDegreeOfParallelism = Environment.ProcessorCount; // تعداد هسته‌های CPU
            var semaphore = new SemaphoreSlim(maxDegreeOfParallelism);

            var tasks = entitiesCopy.Select(async entity =>
            {
                await semaphore.WaitAsync();
                try
                {
                    entity.Update(this); // اجرای کد
                }
                finally
                {
                    semaphore.Release(); // آزاد کردن نخ
                }
            });

            await Task.WhenAll(tasks); // منتظر اتمام همه تسک‌ها


            if (new Random().Next(0, 10) < 2)
            {
                EventManager.TriggerEvent("SeasonChange");
            }

            if (new Random().Next(0, 50) == 1)
            {
                EventManager.TriggerEvent("Earthquake");
            }
        }


        private void OnSeasonChange()
        {
            //form.WriteLine("🍂 تغییر فصل رخ داد! منابع غذا و آب تغییر یافتند.");
            FoodSupply += 50;
            WaterSupply += 30;
        }

        private void OnEarthquake()
        {
            //form.WriteLine("🌋 زلزله رخ داد! منابع کاهش یافتند و برخی موجودات آسیب دیدند.");
            FoodSupply = Math.Max(FoodSupply - 20, 0);
            WaterSupply = Math.Max(WaterSupply - 10, 0);
        }
    }


}
