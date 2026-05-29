using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CycleEngine.Definitions;
using CycleEngine.Entities;
using CycleEngine.Services;
using CycleEngine.Utils;
using Tomlyn;
using Tomlyn.Model;

namespace CycleEngine.Core
{
    public class Bootstrapper
    {
        private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();
        private readonly ResourceManager _resourceManager = new ResourceManager();
        
        private sealed class CycleEngineImpl : CycleEngine
        {
            public CycleEngineImpl(ServiceManager services, ResourceManager resources) : base(services, resources) { }
        }
        
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
        
        private T GetService<T>() where T : IService
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var instance)) {
                return (T)instance;
            }

            throw new CycleServiceNotFoundException(nameof(T));
        }

        public CycleEngine Build()
        {
            var projectConfig = TomlSerializer.Deserialize<ProjectConfig>(GetService<IStorageBackend>().ReadText("/cycleproject.toml"));
            if (projectConfig is null) throw new CycleResourceNotFoundException("cycleproject.toml");
            DeserializeResourceIndex(projectConfig.Assets.Audio);
            DeserializeResourceIndex(projectConfig.Assets.Image);
            DeserializeResourceIndex(projectConfig.Assets.Music);
            DeserializeResourceIndex(projectConfig.Assets.Script);
            DeserializeResourceIndex(projectConfig.Assets.Video);
            DeserializeResourceIndex(projectConfig.Assets.Background);
            DeserializeResourceIndex(projectConfig.Assets.Save);
            
            return new CycleEngineImpl(new ServiceManager(_services), _resourceManager);
        }

        private void DeserializeResourceIndex(string indexPath)
        {
            var doc = TomlSerializer.Deserialize<TomlTable>(GetService<IStorageBackend>().ReadText(indexPath));
        
            foreach (var (categoryName, categoryValue) in doc)
            {
                if (!(categoryValue is TomlTable))
                {
                    continue;
                }
                TomlTable categoryTable = (TomlTable)categoryValue;
                
                foreach (var (key, value) in categoryTable)
                {
                    ResourceType resourceType = ResourceRef.GetType(categoryName);
                    if (resourceType == ResourceType.Undefine)
                        throw new CycleAttributeValueTypeMismatchException("Resource Type", categoryName);
                    
                    if (value is string path)
                    {
                        _resourceManager.RegisterPath(resourceType, key, path);
                    }
                }
            }
        }
    }
}