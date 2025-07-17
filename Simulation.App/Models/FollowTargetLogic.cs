using Simulation.Engine.models;
using System.Numerics;

namespace Simulation.App.Models
{
    public class FollowTargetLogic : ILogic
    {
        private readonly ISimulableObject _target;
        private readonly Queue<Vector2> _previousPositions = new();
        private readonly int _delayFrames;

        public FollowTargetLogic(ISimulableObject target, int delayFrames = 15)
        {
            _target = target;
            _delayFrames = delayFrames;
        }

        public void Apply(ISimulableObject self, IContext context)
        {
            var selfBB = self.GetComponent<BoundingBoxComponent>();
            var targetBB = _target.GetComponent<BoundingBoxComponent>();

            if (selfBB == null || targetBB == null)
                return;

            // اضافه کردن موقعیت فعلی هدف به صف
            _previousPositions.Enqueue(targetBB.Position);

            // اگر صف بیش از حد لازم شد، موقعیت مورد نظر را انتخاب کن
            if (_previousPositions.Count > _delayFrames)
            {
                var delayedPosition = _previousPositions.Dequeue();
                selfBB.Position = delayedPosition;
            }
        }
    }

}