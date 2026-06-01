using CycleEngine.Core;
using CycleEngine.Definitions;
using CycleEngine.Utils;

namespace CycleEngine
{
    
    /// <summary>
    /// Cycle Engine Instance.
    /// Please use <see cref="Bootstrapper"/> to initialize the instance
    /// </summary>
    public class CycleEngine
    { 
        public ServiceManager Services { get; }
        public EntityManager Entities { get; }
        public ResourceManager Resources { get; }
        public GameConfig Config { get; }
        public EngineTime Time { get; }
        public AnimatableEntityAnimator AnimatableEntityAnimator { get; }
        private ScriptExecutor _executor;

        protected CycleEngine(ServiceManager services, ResourceManager resources)
        {
            Services = services;
            Entities = new EntityManager();
            Resources = resources;
            Config = new GameConfig();
            Time = new EngineTime();
            AnimatableEntityAnimator = new AnimatableEntityAnimator();

            _executor = new ScriptExecutor(this);
        }

        public void NewGame()
        {
            _executor.LoadScript("init");
        }

        public void SaveCurrentGame()
        {
            
        }

        public void EndGame()
        {
            
        }

        public void LoadGame()
        {
            
        }

        /// <summary>
        /// Advances the engine by the given delta time in milliseconds.
        /// Called once per frame by the host.
        /// </summary>
        public void Update(double deltaMs)
        {
            Time.Advance(deltaMs);
        }
        
    }
}