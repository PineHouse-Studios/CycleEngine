using CycleEngine.Definitions.Deserialize;
using CycleEngine.Interpreter;

namespace CycleEngine.Entities
{
    public class Image : EntityBase
    {
        public string Content { get; set; }
        public int Layer { get; set; }
        public bool Show { get; set; }
        
        public string Ease { get; set; }
        public double Dur { get; set; }
        
        public AnimatedVelocityValue<double> X { get; set; }
        public AnimatedVelocityValue<double> Y { get; set; }
        public AnimatedValue<double> Alpha { get; set; }
        public AnimatedValue<double> Zoom { get; set; }

        public AnimatedValue<string> In { get; set; }
        public AnimatedValue<string> Out { get; set; }

        public Image(ImageDeserialize deserialized)
        {
            Content = deserialized.Content;
            Layer = deserialized.Layer;
            Show = deserialized.Show;
            Ease = deserialized.Ease;
            Dur = deserialized.Dur;
            X = deserialized.X;
            Y = deserialized.Y;
            Alpha = deserialized.Alpha;
            Zoom = deserialized.Zoom;
            In = deserialized.In;
            Out = deserialized.Out;
        }
    }
}