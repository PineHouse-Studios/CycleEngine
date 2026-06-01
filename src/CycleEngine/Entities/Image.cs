using System;
using CycleEngine.Definitions;
using CycleEngine.Definitions.Manifest;
using CycleEngine.Interpreter;
using CycleEngine.Utils;

namespace CycleEngine.Entities
{
    public class Image : AnimatableEntity
    {
        public string Content { get; set; }
        public int Layer { get; set; }
        
        public EaseType Ease { get; set; }
        public double Dur { get; set; }

        public Image(string entityKey, ImageDeserialize deserialized) : base(entityKey, deserialized.X, deserialized.Y, deserialized.Rotation, deserialized.Alpha, deserialized.Zoom)
        {
            Content = deserialized.Content;
            Layer = deserialized.Layer;
            Ease = Easing.GetType(deserialized.Ease);
            Dur = deserialized.Dur;
        }

        public void Update(EngineTime time)
        {
            
        }

        public override Entity Copy()
        {
            throw new NotImplementedException();
        }
    }
}