using System;
using System.Collections.Generic;
using CycleEngine.Definitions;
using CycleEngine.Services;
using CycleEngine.Utils;
using Tomlyn;
using Tomlyn.Model;

namespace CycleEngine.Core
{
    public class Bootstrapper
    {
        private readonly Dictionary<Type, IService> _services = new();
        private readonly ResourceManager _resourceManager = new();
        
        private sealed class CycleEngineImpl(Dictionary<Type, IService> services, ResourceManager resources)
            : CycleEngine(services, resources) { }
        
        /// <summary>
        /// Registers a service instance. All services should be registered during initialization of the engine instance by bootstrapper. <br/>
        /// 
        /// IMPORTANT: When registering an implementation under an interface,
        /// you must specify the service type explicitly via the generic parameter.
        /// C# type inference will otherwise pick the concrete class, making
        /// Get&lt;IInterface&gt;() fail.
        /// </summary>
        /// <typeparam name="T">Interface of the service</typeparam>
        /// <param name="instance">Instance of the service</param>
        public Bootstrapper RegisterService<T>(T instance) where T : IService
        {
            var type = typeof(T);
            _services[type] = instance;
            return this;
        }

        public CycleEngine Build()
        {
            IStorageBackend storage;
            if(_services.TryGetValue(typeof(IStorageBackend), out var service))
            {
                storage = (IStorageBackend) service;
            }
            else
            {
                throw new CycleServiceNotFoundException("Storage");
            }

            string? raw = storage.ReadText("/cycleproject.toml");
            if (raw is null)
            {
                throw new CycleFileNotFoundException("/cycleproject.toml");
            }
            var projectConfig = TomlSerializer.Deserialize<ProjectConfig>(raw);
            if (projectConfig is null) throw new CycleSyntaxException("cycleproject.toml");
            DeserializeResourceIndex(projectConfig.Assets.Audio + "/index.toml", storage);
            DeserializeResourceIndex(projectConfig.Assets.Image + "/index.toml", storage);
            DeserializeResourceIndex(projectConfig.Assets.Music + "/index.toml", storage);
            DeserializeResourceIndex(projectConfig.Assets.Script + "/index.toml", storage);
            DeserializeResourceIndex(projectConfig.Assets.Video + "/index.toml", storage);
            DeserializeResourceIndex(projectConfig.Assets.Background + "/index.toml", storage);
            DeserializeResourceIndex(projectConfig.Assets.Save + "/index.toml", storage);
            
            return new CycleEngineImpl(_services, _resourceManager);
        }

        private void DeserializeResourceIndex(string indexPath, IStorageBackend storageBackend)
        {
            string? raw = storageBackend.ReadText(indexPath);
            if (raw is null)
            {
                throw new CycleFileNotFoundException(indexPath);
            }
            var doc = TomlSerializer.Deserialize<TomlTable>(raw);
            if (doc is null) throw new CycleSyntaxException(indexPath);
        
            foreach (var (categoryName, categoryValue) in doc)
            {
                if (categoryValue is not TomlTable categoryTable)
                {
                    continue;
                }
                
                foreach (var (key, value) in categoryTable)
                {
                    ResourceType resourceType = ResourceRef.GetType(categoryName);
                    if (resourceType == ResourceType.Undefine)
                        throw new CycleTypeMismatchException("Resource Type", categoryName);
                    
                    if (value is string path)
                    {
                        _resourceManager.RegisterPath(resourceType, key, path);
                    }
                }
            }
        }
    }
}
