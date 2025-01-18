using System;
using System.Collections.Generic;
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


















    public class World
    {
        public string Name { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public int FoodSupply { get; set; }
        public int WaterSupply { get; set; }
        public List<LivingBeing> Entities { get; set; } = [];
        public List<LivingBeing> DiedEntities { get; set; } = [];
        public List<LivingBeing> NewEntities { get; set; } = [];

        public EventManager EventManager { get; set; } = new EventManager();

        public Output Output { get; set; } = new Output();

        public World(string name, int width, int height, int initialFood, int initialWater)
        {
            Name = name;
            Width = width;
            Height = height;
            FoodSupply = initialFood;
            WaterSupply = initialWater;

            EventManager.RegisterEvent("SeasonChange", OnSeasonChange);
            EventManager.RegisterEvent("Earthquake", OnEarthquake);
        }

        public void AddEntity(LivingBeing entity)
        {
            if (entity.Location.X < 0 || entity.Location.X >= Width || entity.Location.Y < 0 || entity.Location.Y >= Height)
            {
                throw new ArgumentException("مکان موجود زنده خارج از محدوده زمین است!");
            }

            Entities.Add(entity);
            EventManager.SimulationEntities.Add(entity);
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
