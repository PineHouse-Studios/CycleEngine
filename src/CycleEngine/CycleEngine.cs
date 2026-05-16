using CycleEngine.Core;
using CycleEngine.Definitions;

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

        protected CycleEngine(ServiceManager services, ResourceManager resources)
        {
            Services = services;
            Entities = new EntityManager();
            Resources = resources;
            Config = new GameConfig();
        }

        public void NewGame()
        {
            
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
        
    }
}