using Simulation.Engine.Components.physic;
using Simulation.Engine.models;
using System.Drawing;

namespace Simulation.App.Models
{
    public class BoundsCollisionLogic : ILogic
    {
        // شرطی برای تعیین اینکه آیا برخورد بین دو شی مجاز به بررسی است یا نه
        public Func<string?, string?, bool>? TagCondition { get; set; }

        // عملی که پس از تشخیص برخورد باید انجام شود
        public Action<ISimulableObject, ISimulableObject>? OnCollision { get; set; }

        public void Apply(ISimulableObject simulableObject, IContext context)
        {
            var allObjects = context.Objects;

            for (int i = 0; i < allObjects.Count; i++)
            {
                var objA = allObjects[i];
                var boundsA = objA.GetComponent<IHasBounds>();
                if (boundsA == null)
                    continue;

                var rectA = GetRect(boundsA);
                var tagA = GetTag(objA);

                for (int j = i + 1; j < allObjects.Count; j++)
                {
                    var objB = allObjects[j];
                    var boundsB = objB.GetComponent<IHasBounds>();
                    if (boundsB == null)
                        continue;

                    var rectB = GetRect(boundsB);
                    var tagB = GetTag(objB);

                    // بررسی شرط تگ‌ها (اختیاری)
                    if (TagCondition != null && !TagCondition(tagA, tagB))
                        continue;

                    // بررسی برخورد
                    if (rectA.IntersectsWith(rectB))
                    {
                        OnCollision?.Invoke(objA, objB);
                    }
                }
            }
        }

        private RectangleF GetRect(IHasBounds bounds)
        {
            return new RectangleF(bounds.Position.X, bounds.Position.Y, bounds.Size.X, bounds.Size.Y);
        }

        private string? GetTag(ISimulableObject obj)
        {
            if (obj is ITaggable taggable)
                return taggable.Tag;
            return null;
        }
    }

    public interface ITaggable
    {
        public string? Tag { get; set; }
    }


}
