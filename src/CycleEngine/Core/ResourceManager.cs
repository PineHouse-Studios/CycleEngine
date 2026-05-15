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
        private readonly Dictionary<Resource, Dictionary<string, string>> _resources =
            new Dictionary<Resource, Dictionary<string, string>>();
        
        public void RegisterPath(Resource type, string key, string absolutePath)
        {
            _resources[type][key] = absolutePath;
        }
    }
}