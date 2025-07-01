using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Engine.Components.Render
{
    public interface IRenderable : IComponent
    {
        /// <summary>
        /// تصویر (یا نام تصویر) مرتبط با شیء.
        /// می‌تونه نام یک تکسچر در مونوگیم باشه، یا شناسه‌ای برای نوع شکل (مثلاً "circle", "square").
        /// </summary>
        string SpriteId { get; set; }

        /// <summary>
        /// موقعیت شیء روی صفحه.
        /// می‌تونیم فرض کنیم که این مختصات از خود شیء گرفته می‌شه، ولی برای ساده‌سازی اینجا هم تعریف می‌کنیم.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// اندازه‌ی نمایش شیء (مثلاً شعاع توپ یا عرض تصویر)
        /// </summary>
        Vector2 Size { get; set; }

        /// <summary>
        /// رنگ شیء برای رسم در MonoGame (در صورت استفاده نکردن از sprite واقعی).
        /// </summary>
        Color Color { get; set; }
    }

}
