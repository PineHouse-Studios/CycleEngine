using System;
using System.Collections.Generic;
using CycleEngine.Utils;

namespace CycleEngine.Core
{
    public static class ServiceManager
    {
        private static readonly Dictionary<Type, object> Serv = new Dictionary<Type, object>();
        
        public static void Register<T>(T service) where T : class
        {
            var type = typeof(T);
            Serv[type] = service;
        }
        
        public static T Get<T>() where T : class
        {
            var type = typeof(T);
            if (Serv.TryGetValue(type, out var service)) {
                return (T)service;
            }

            throw new CycleServiceNotFoundException(nameof(T));
        }
    }
}