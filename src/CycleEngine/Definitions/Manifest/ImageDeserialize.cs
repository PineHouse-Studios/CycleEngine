using System;
using CycleEngine.Interpreter;
using CycleEngine.Utils;

namespace CycleEngine.Definitions.Manifest
{
    public class ImageDeserialize
    {
        public string Content { get; set; } = String.Empty;
        public int Layer { get; set; }
        public bool Show { get; set; }

        public string Ease { get; set; } = String.Empty;
        public double Dur { get; set; }
        
        public AnimatedValue<double> X { get; set; } = new();
        public AnimatedValue<double> Y { get; set; } = new();
        public AnimatedValue<double> Rotation { get; set; } = new();
        public AnimatedValue<double> Alpha { get; set; } = new() { Value = 100 };
        public AnimatedValue<double> Zoom { get; set; } = new() { Value = 100 };
    }
}