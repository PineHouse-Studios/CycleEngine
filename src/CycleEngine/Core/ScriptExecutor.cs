using CycleEngine.Definitions;
using CycleEngine.Interfaces;
using CycleEngine.Utils;

namespace CycleEngine.Core
{
    public class ScriptExecutor
    {
        private CycleEngineScript _currentScript;
        private CycleEngine _engine;

        public ScriptExecutor(CycleEngine engine)
        {
            _engine = engine;
        }

        public void LoadScript(string key)
        {
            string scriptPath = _engine.Resources.GetPath(ResourceType.CycleEngineScripts, key);
            string scriptRaw = _engine.Services.Get<ICycleProjectStorage>().ReadText(scriptPath);
        }
    }
}