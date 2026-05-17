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
        
        public void Register(string key, Entity entity)
        {
            _entities[key] = entity;
        }
        
        public Entity Get(string key)
        {
            if (_entities.TryGetValue(key, out var entity))
            {
                return entity;
            }

            throw new CycleEntityNotFoundException(key);
        }
        
        public void Clear()
        {
            _entities.Clear();
        }
    }
}