using CycleEngine.Script.Attributes;
using CycleEngine.Script.Types;

namespace CycleEngine.Script.Examples;

[CycleChapter]
public partial class MyCycleScript : CycleScript
{
    public override void OnLoad()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }

    [CycleEvent]
    private void E1()
    {

    }
    
    [CycleEvent]
    private void Ending()
    {
    }
}
