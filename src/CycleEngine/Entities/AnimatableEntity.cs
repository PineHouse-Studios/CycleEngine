using CycleEngine.Definitions;
using CycleEngine.Utils;

namespace CycleEngine.Entities
{
    public class AnimatableEntity : Entity
    {
        public AnimatableEntity(string entityKey, AnimatedValue<double> x, AnimatedValue<double> y, AnimatedValue<double> rotation, AnimatedValue<double> alpha, AnimatedValue<double> zoom) : base(entityKey)
        {
            X = x;
            Y = y;
            Rotation = rotation;
            Alpha = alpha;
            Zoom = zoom;
        }

        public AnimatedValue<double> X { get; set; }
        public AnimatedValue<double> Y { get; set; }
        public AnimatedValue<double> Rotation { get; set; }
        public AnimatedValue<double> Alpha { get; set; }
        public AnimatedValue<double> Zoom { get; set; }

        public static AnimatableEntity Copy(AnimatableEntity animatableEntity)
        {
            return new AnimatableEntity(
                animatableEntity.EntityKey, 
                AnimatedValue<double>.Copy(animatableEntity.X),
                AnimatedValue<double>.Copy(animatableEntity.Y),
                AnimatedValue<double>.Copy(animatableEntity.Rotation),
                AnimatedValue<double>.Copy(animatableEntity.Alpha),
                AnimatedValue<double>.Copy(animatableEntity.Zoom)
                );
        }
    }
}