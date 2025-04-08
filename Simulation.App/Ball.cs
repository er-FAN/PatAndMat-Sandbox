
using Simulation.Engine.events;
using Simulation.Engine.models;
using System;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Simulation.App.SimulationObjects
{
    public class Ball : ISimulableObject, IHasBehaviors, IHasRelations, IHasEventListener
    {
        public Guid Id { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double VX { get; set; }
        public double VY { get; set; }
        public double Radius { get; set; }
        public Ellipse shape { get; set; }
        public List<IBehavior> Behaviors { get; } = [];
        public List<IRelation> Relations { get; } = [];
        public List<IEventListener> EventListeners { get; set; } = [];
        public List<Brush> Brushess { get; set; } = [];
        public Ball(double x, double y, double vx, double vy, double radius)
        {
            Brushess.Add(Brushes.Blue);
            Brushess.Add(Brushes.Red);
            Brushess.Add(Brushes.Green);
            Brushess.Add(Brushes.Yellow);
            Brushess.Add(Brushes.Orange);
            Brushess.Add(Brushes.Purple);
            Brushess.Add(Brushes.White);
            X = x;
            Y = y;
            VX = vx;
            VY = vy;
            Radius = radius;
            shape = new Ellipse
            {
                Width = Radius * 2,
                Height = Radius * 2,
                Fill = Brushes.Red
            };
        }
    }


    public class MovementBehavior : IBehavior
    {
        public void Apply(ISimulableObject obj, SimulationContext context)
        {
            if (obj is Ball ball)
            {
                ball.X += ball.VX;
                ball.Y += ball.VY;
            }
        }
    }

    public class WallBounceBehavior : IBehavior
    {
        private double Width;
        private double Height;

        // سازنده که عرض و ارتفاع را به آن ارسال می‌کنیم
        public WallBounceBehavior(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public void Apply(ISimulableObject obj, SimulationContext context)
        {
            Random random = new Random();
            if (obj is Ball ball)
            {
                // دیوارهای اطراف
                if (ball.X - ball.Radius < 0 || ball.X + ball.Radius > Width)
                {
                    ball.VX = -ball.VX; // تغییر جهت به‌صورت افقی
                    ball.shape.Fill = ball.Brushess[random.Next(0, 6)];
                }
                    
                if (ball.Y - ball.Radius < 0 || ball.Y + ball.Radius > Height)
                {
                    ball.VY = -ball.VY; // تغییر جهت به‌صورت عمودی
                    
                }
                    
            }
        }

        public void UpdateDimensions(double newWidth, double newHeight)
        {
            Width = newWidth;
            Height = newHeight;
        }
    }

    public class BallCollisionBehavior(List<Ball> Balls) : IBehavior
    {
        public double Elasticity { get; set; } = 1.0; // مقدار الاستیسیته برخورد (مقدار 1 برای برخورد الاستیک کامل)
        public List<Ball> Balls { get; set; } = Balls;
        public void Apply(ISimulableObject obj, SimulationContext context)
        {
            Random random = new Random();
            if (obj is Ball ball1)
            {
                foreach (var ball2 in Balls.Where(b => b != ball1))
                {
                    double dx = ball2.X - ball1.X;
                    double dy = ball2.Y - ball1.Y;
                    double distance = Math.Sqrt(dx * dx + dy * dy);
                    double minDistance = ball1.Radius + ball2.Radius;

                    if (distance < minDistance)
                    {
                        // برخورد
                        double angle = Math.Atan2(dy, dx);
                        double sin = Math.Sin(angle);
                        double cos = Math.Cos(angle);

                        // تبدیل به مختصات محورهای توپ‌ها
                        var ball1NewVelocity = new { VX = ball1.VX * cos + ball1.VY * sin, VY = ball1.VY * cos - ball1.VX * sin };
                        var ball2NewVelocity = new { VX = ball2.VX * cos + ball2.VY * sin, VY = ball2.VY * cos - ball2.VX * sin };

                        // به‌روزرسانی سرعت‌ها (سرعت‌ها را معکوس می‌کنیم چون توپ‌ها برخورد کرده‌اند)
                        double tempVX = ball1NewVelocity.VX;
                        ball1.VX = ball2NewVelocity.VX * Elasticity;
                        ball2.VX = tempVX * Elasticity;

                        double tempVY = ball1NewVelocity.VY;
                        ball1.VY = ball2NewVelocity.VY * Elasticity;
                        ball2.VY = tempVY * Elasticity;

                        // به‌روزرسانی موقعیت توپ‌ها به‌گونه‌ای که از هم دور شوند (از هم جدا شوند)
                        double overlap = minDistance - distance;
                        ball1.X -= overlap * cos / 2;
                        ball1.Y -= overlap * sin / 2;
                        ball2.X += overlap * cos / 2;
                        ball2.Y += overlap * sin / 2;
                        ball1.shape.Fill = ball1.Brushess[random.Next(0, 6)];
                        ball2.shape.Fill = ball2.Brushess[random.Next(0, 6)];
                    }
                }
            }
        }
    }
}