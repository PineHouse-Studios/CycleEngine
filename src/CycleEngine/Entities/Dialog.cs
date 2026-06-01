using CycleEngine.Definitions;
using CycleEngine.Entities;

namespace CycleEngine.Entities
{
    /// <summary>
    /// Dialog entity is a singleton instance in a cycle engine instance
    /// </summary>
    public class Dialog : Entity
    {
        public Dialog(string entityKey, string? title, string? text, string[]? option) : base(entityKey)
        {
            Title = title;
            Text = text;
            Option = option;
        }

        public string? Title { set; get; }
        public string? Text { set; get; }
        public string[]? Option { set; get; }
        public override Entity Copy()
        {
            throw new System.NotImplementedException();
        }
    }
}