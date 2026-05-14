using CycleEngine.Core;

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

        protected CycleEngine(ServiceManager services, ResourceManager resources)
        {
            Services = services;
            Resources = resources;
            Entities = new EntityManager();
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