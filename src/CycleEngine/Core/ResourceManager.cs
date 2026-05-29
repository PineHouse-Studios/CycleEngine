using System;
using System.Collections.Generic;
using CycleEngine.Definitions;
using CycleEngine.Entities;
using CycleEngine.Utils;

namespace CycleEngine.Core
{
    /// <summary>
    /// Manage ResourceRef Paths and Keys
    /// </summary>
    public class ResourceManager
    {
        private readonly Dictionary<ResourceType, Dictionary<string, string>> _resources =
            new Dictionary<ResourceType, Dictionary<string, string>>();
        
        public void RegisterPath(ResourceType type, string key, string absolutePath)
        {
            _resources[type][key] = absolutePath;
        }

        public string? GetPath(ResourceType type, string key)
        {
            if (_resources.TryGetValue(type, out var entries) && entries.TryGetValue(key, out var foundPath))
            {
                return foundPath;
            }
            return null;
        }
    }
}