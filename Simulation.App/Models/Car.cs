using Simulation.Engine.events;
using Simulation.Engine.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.App.Models
{
    public class Car : ISimulableObject, IHasRelations, IHasBehaviors, IHasEvent, IHasEventListener
    {
        public Guid Id { get; set; }
        public int Speed { get; set; }
        public Node Location { get; set; } = new Node();
        public double X { get; set; }
        public double Y { get; set; }
        public Node Goal { get; set; } = new Node();
        public List<Node> Route { get; set; } = [];

        public List<IRelation> Relations { get; set; } = new List<IRelation>();

        public List<IBehavior> Behaviors { get; } = [];

        public EventBus EventBus { get; set; }
        public List<ISimulationEvent> Events { get; set; } = [];

        public List<IEventListener> EventListeners { get; set; } = [];

        private Graph _graph; // گراف خیابان‌ها

        public Car(EventBus eventBus, Graph graph)
        {
            EventBus = eventBus;
            _graph = graph;
        }

        // مسیریابی
        public void CalculateRoute()
        {
            var route = _graph.FindRoute(new Node { X = Location.X, Y = Location.Y }, Goal);

            if (route != null)
            {
                // مسیر پیدا شده است
                this.Route = route.Select(street => street.Start).ToList();
            }
            else
            {
                // مسیری برای رسیدن به هدف پیدا نشد
                this.Route.Clear();
            }
        }
    }

    public class MoveBehavior : IBehavior
    {
        public void Apply(ISimulableObject obj, SimulationContext context)
        {
            // بررسی نوع شیء و اطمینان از اینکه شیء مورد نظر از نوع Car یا هر شیء متحرک دیگری است
            if (obj is Car car)
            {
                // سرعت ماشین را از طریق context یا شیء دریافت می‌کنیم (در اینجا فرض کرده‌ایم سرعت به طور مستقیم از Car گرفته می‌شود)
                double speed = car.Speed;

                // محاسبه جهت حرکت ماشین (در اینجا به سمت اولین نود از Route حرکت می‌کنیم)
                if (car.Route.Count > 0)
                {
                    var targetNode = car.Route[0]; // هدف ماشین (اولین نود از Route)
                    double dx = targetNode.X - car.X;
                    double dy = targetNode.Y - car.Y;
                    double distance = Math.Sqrt(dx * dx + dy * dy);

                    // اگر ماشین به هدف رسید، به نود بعدی می‌رود
                    if (distance < speed)
                    {
                        // رسیدن به هدف
                        car.X = targetNode.X;
                        car.Y = targetNode.Y;

                        // حذف نود از مسیر (اگر به هدف رسید)
                        car.Route.RemoveAt(0);
                    }
                    else
                    {
                        // محاسبه زاویه حرکت ماشین به سمت نود هدف
                        double angle = Math.Atan2(dy, dx);

                        // حرکت ماشین به سمت نود هدف
                        car.X += speed * Math.Cos(angle);
                        car.Y += speed * Math.Sin(angle);
                    }
                }
            }
        }
    }
}
