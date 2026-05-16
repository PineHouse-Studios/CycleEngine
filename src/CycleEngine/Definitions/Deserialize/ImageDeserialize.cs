using CycleEngine.Interpreter;

namespace CycleEngine.Definitions.Deserialize
{
    public class ImageDeserialize
    {
        public string Content { get; set; } = "";
        public int Layer { get; set; }
        public bool Show { get; set; }
        
        public string Ease { get; set; } = "fade";
        public double Dur { get; set; }
        
        public AnimatedVelocityValue<double> X { get; set; } = new AnimatedVelocityValue<double>();
        public AnimatedVelocityValue<double> Y { get; set; } = new AnimatedVelocityValue<double>();
        public AnimatedValue<double> Alpha { get; set; } = new AnimatedValue<double>() { Value = 100 };
        public AnimatedValue<double> Zoom { get; set; } = new AnimatedValue<double>() { Value = 100 };

        public AnimatedValue<string> In { get; set; } = new AnimatedValue<string>() { Value = "fade" };
        public AnimatedValue<string> Out { get; set; } = new AnimatedValue<string>() { Value = "fade" };
    }
}