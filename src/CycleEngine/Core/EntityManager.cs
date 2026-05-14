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
        private readonly Dictionary<string, EntityBase> _entities = new Dictionary<string, EntityBase>();
        
        public void Register(string key, EntityBase entity)
        {
            _entities[key] = entity;
        }
        
        public EntityBase Get(string key)
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