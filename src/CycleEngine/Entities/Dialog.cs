using CycleEngine.Entities;

namespace CycleEngine.Entities
{
    /// <summary>
    /// Dialog entity is a singleton instance in a cycle engine instance
    /// </summary>
    public class Dialog : Entity
    {
        public string? Title { set; get; }
        public string? Text { set; get; }
        public string[]? Option { set; get; }
    }
}