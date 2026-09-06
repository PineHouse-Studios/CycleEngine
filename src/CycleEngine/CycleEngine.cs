using System;
using System.Collections.Generic;
using CycleEngine.Core;
using CycleEngine.Definitions;
using CycleEngine.Services;
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
        private EngineTime _time;

        protected CycleEngine(Dictionary<Type, IService> services, ResourceManager resources)
        {
            Entities = new EntityManager();
            Resources = resources;

            Dictionary<Type, IService> active = new Dictionary<Type, IService>();
            foreach (var type in services)
            {
                active[type.Key] = type.Value;
            }

            active[typeof(GameConfig)] = new GameConfig();
            _time = new EngineTime();
            active[typeof(EngineTime)] = _time;
            active[typeof(AnimatableEntityAnimator)] = new AnimatableEntityAnimator(this);
            active[typeof(ScriptExecutor)] = new ScriptExecutor(this);

            Services = new ServiceManager(active);
        }

        public void NewGame()
        {
            Services.Get<ScriptExecutor>().LoadScript("init");
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
            _time.Advance(deltaMs);
            Services.Get<AnimatableEntityAnimator>().Update(_time);
        }
        
    }
}