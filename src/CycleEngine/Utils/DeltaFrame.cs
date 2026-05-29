using System.Collections.Generic;

namespace CycleEngine.Utils
{
    public class DeltaFrame
    {
        /// <summary>
        /// Changes on common properties of entities in one frame <br/>
        /// string, string, double <br/>
        /// First string -> Entity Key <br/>
        /// Second string -> attribute (x, y, zoom, layer, rotation, alpha)<br/>
        /// double -> new value <br/>
        /// </summary>
        public Dictionary<string, Dictionary<string, double>> EntityChanges { get; } = new Dictionary<string, Dictionary<string, double>>();

        internal DeltaFrame AppendChange(string entityId, string property, double value)
        {
            EntityChanges[entityId].Add(property, value);
            return this;
        }
        
        public bool IsEmpty => EntityChanges.Count == 0;
    }
}