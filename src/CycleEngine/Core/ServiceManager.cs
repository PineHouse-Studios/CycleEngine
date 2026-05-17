using System;
using System.Collections.Generic;
using CycleEngine.Services;
using CycleEngine.Utils;

namespace CycleEngine.Core
{
    public class ServiceManager
    {
        // All services should be registered during initialization of the engine instance by bootstrapper
        private readonly Dictionary<Type, IService> _services;

        public ServiceManager(Dictionary<Type, IService> services)
        {
            _services = services;
        }
        
        public T Get<T>() where T : IService
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var instance)) {
                return (T)instance;
            }

            throw new CycleServiceNotFoundException(nameof(T));
        }
    }
}