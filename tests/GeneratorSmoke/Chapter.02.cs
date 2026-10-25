using CycleEngine.Script.Attributes;

namespace CycleEngine.GeneratorSmoke;

public partial class SplitChapter
{
    [CycleEvent]
    private void Ending() => Calls.Add("Ending");
}
