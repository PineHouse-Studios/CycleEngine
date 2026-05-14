using System.Collections.Generic;
using CycleEngine.Entities;
using CycleEngine.Utils;

namespace CycleEngine.Core
{
    /// <summary>
    /// Manage Resource Paths and Keys
    /// </summary>
    public class ResourceManager
    {
        private readonly Dictionary<string, string> _resources = new Dictionary<string, string>();
        
        public void RegisterPath(string key, string absolutePath)
        {
            _resources[key] = absolutePath;
        }
        
        public string GetPath(string key)
        {
            if (_resources.TryGetValue(key, out var path))
            {
                return path;
            }

            throw new CycleEntityNotFoundException(key);
        }
    }
}