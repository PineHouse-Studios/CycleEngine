using CycleEngine.Definitions;
using CycleEngine.Entities;
using CycleEngine.Utils;

namespace CycleEngine.Services
{
    public interface IRenderBackend : IService
    {
        /// <summary>
        /// Creates an entity
        /// </summary>
        /// <param name="type">Type of the entity: Image/Audio/Music/Video/Background</param>
        /// <param name="entityId">Entity ID, should be unique</param>
        void CreateEntity(string type, string entityId);
        
        /// <summary>
        /// Destroy an entity
        /// </summary>
        void DestroyEntity(string entityId);

        /// <summary>
        /// Update and render states of entities
        /// </summary>
        void ApplyFrame(DeltaFrame<Entity> delta);
    }
}