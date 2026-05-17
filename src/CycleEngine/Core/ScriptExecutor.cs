using CycleEngine.Definitions;
using CycleEngine.Interpreter;
using CycleEngine.Services;
using CycleEngine.Utils;

namespace CycleEngine.Core
{
    public class ScriptExecutor
    {
        private CycleEngineScript? _currentScript;
        private readonly CycleEngine _engine;

        public ScriptExecutor(CycleEngine engine)
        {
            _engine = engine;
        }

        public void LoadScript(string key)
        {
            _engine.Entities.Clear();
            string scriptPath = _engine.Resources.GetPath(ResourceType.CycleEngineScript, key);
            string scriptRaw = _engine.Services.Get<ICycleProjectStorage>().ReadText(scriptPath);
            _currentScript = CycleEngineScriptParser.Parse(scriptRaw);
        }
    }
}