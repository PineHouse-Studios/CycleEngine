using System;
using System.Collections.Generic;
using CycleEngine.Entities;
using CycleEngine.Utils;

namespace CycleEngine.Core
{
    /// <summary>
    /// Runtime entities
    /// </summary>
    public class EntityManager
    {
        private readonly Dictionary<string, Entity> _entities = new Dictionary<string, Entity>();
        private readonly Dictionary<Type, Entity> _singletons = new Dictionary<Type, Entity>();
        
        public void Register(string key, Entity entity)
        {
            _entities[key] = entity;
        }
        
        public Entity? Get(string key)
        {
             return _entities.TryGetValue(key, out var entity) ? entity : null;
        }

        public void RegisterSingleton<T>(T entity) where T : Entity
        {
            var type = typeof(T);
            _singletons[type] = entity;
        }

        public T? GetSingleton<T>() where T : Entity
        {
            var type = typeof(T);
            if (_singletons.TryGetValue(type, out var instance)) {
                return (T)instance;
            }

            return null;
        }
        
        public void Clear()
        {
            _entities.Clear();
        }
    }
}