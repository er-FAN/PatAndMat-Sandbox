using System;
using System.Collections.Generic;
using System.Drawing;

public class Lake
{
    private Random random = new Random();

    // ویژگی‌ها
    public PointF Location { get; set; } // مرکز دریاچه
    public float Width { get; set; }
    public float Height { get; set; }
    public List<PointF> Points { get; private set; } // نقاط منحنی بسته

    // سازنده کلاس
    public Lake(float x, float y, float width, float height)
    {
        Location = new PointF(x, y);
        Width = width;
        Height = height;
        Points = new List<PointF>();

        // تولید شکل رندوم برای دریاچه
        GenerateRandomShape();
    }

    // متدی برای تولید شکل رندوم
    public void GenerateRandomShape()
    {
        int pointCount = random.Next(5, 10); // تعداد نقاط بیشتر برای پیچیدگی بیشتر
        Points.Clear();
        for (int i = 0; i < pointCount; i++)
        {
            double angle = (2 * Math.PI / pointCount) * i;
            float radius = (float)(random.NextDouble() * (Width / 2)); // شعاع تصادفی
            float px = Location.X + (float)(Math.Cos(angle) * radius);
            float py = Location.Y + (float)(Math.Sin(angle) * radius);
            Points.Add(new PointF(px, py));
        }
    }
}
