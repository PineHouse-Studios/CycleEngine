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
            string? scriptPath = _engine.Resources.GetPath(ResourceType.CycleEngineScript, key);
            if (scriptPath is null) throw new CycleResourceNotFoundException($"{key} missing in script index file");
            
            string? scriptRaw = _engine.Services.Get<ICycleProjectStorage>().ReadText(scriptPath);
            if (scriptRaw is null) throw new CycleResourceNotFoundException($"key -> {key}, path -> {scriptPath}");
            
            _currentScript = CycleEngineScriptParser.Parse(scriptRaw);
        }

        public async void Run()
        {
            
        }
    }
}