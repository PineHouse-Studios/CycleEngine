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

        public override Entity Copy()
        {
            return new AnimatableEntity(
                EntityKey, 
                AnimatedValue<double>.Copy(X),
                AnimatedValue<double>.Copy(Y),
                AnimatedValue<double>.Copy(Rotation),
                AnimatedValue<double>.Copy(Alpha),
                AnimatedValue<double>.Copy(Zoom)
            );
        }
    }
}