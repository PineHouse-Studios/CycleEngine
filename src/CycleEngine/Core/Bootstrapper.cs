using System;
using System.Collections.Generic;
using CycleEngine.Entities;

namespace CycleEngine.Core
{
    public class Bootstrapper
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        
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
        public Bootstrapper RegisterService<T>(T instance) where T : class
        {
            var type = typeof(T);
            _services[type] = instance;
            return this;
        }

        public CycleEngine Build(string projectPath)
        {
            ResourceManager resourceManager = new ResourceManager();
            
            // TODO implement load project
            
            return new CycleEngineImpl(new ServiceManager(_services), resourceManager);
        }
    }
}