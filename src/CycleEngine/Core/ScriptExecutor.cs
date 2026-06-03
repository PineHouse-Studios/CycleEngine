using System.Threading.Tasks;
using CycleEngine.Commands;
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
            
            string? scriptRaw = _engine.Services.Get<IStorageBackend>().ReadText(scriptPath);
            if (scriptRaw is null) throw new CycleResourceNotFoundException($"key -> {key}, path -> {scriptPath}");
            
            _currentScript = CycleEngineScriptParser.Parse(scriptRaw);
        }

        public async void Run()
        {
            if (_currentScript is null) return;
            if (_currentScript.Attributes.TryGetValue("title", out string title))
            {
                _engine.Services.Get<ISystemBackend>().UpdateWindowTitle(title);
            }
            RunCommands(_currentScript.Body["script"].NamedBlocks["onload"].Commands);
        }

        private void RunBlock(ScriptBlock block)
        {
            
        }

        
        private async Task RunCommands(Command[] commands)
        {
            for (int line = 0; line < commands.Length; line++)
            {
                if (commands[line] is WaitCommand)
                {
                    await commands[line].AsyncExecute();
                }
                else
                {
                    commands[line].AsyncExecute();
                }
                
            }
        }
    }
}