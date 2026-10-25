using CycleEngine.Script.Attributes;

namespace CycleEngine.Script.Types;

public abstract class CycleScript
{
    public abstract void OnLoad();
    
    public abstract void OnExit();

    public abstract CycleScriptConfig ExportConfig();
}