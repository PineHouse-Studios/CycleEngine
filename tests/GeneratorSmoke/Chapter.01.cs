using System.Collections.Generic;
using CycleEngine.Script.Attributes;
using CycleEngine.Script.Types;

namespace CycleEngine.GeneratorSmoke;

[CycleChapter("prologue")]
public partial class SplitChapter : CycleScript
{
    public List<string> Calls { get; } = new();
    public override void OnLoad() { }
    public override void OnExit() { }

    [CycleEvent]
    private void Zebra() => Calls.Add("Zebra");

    [CycleEvent]
    private void Alpha() => Calls.Add("Alpha");
}
