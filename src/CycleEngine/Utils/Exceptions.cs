using CycleEngine.Core;
using System;

namespace CycleEngine.Utils
{
    public class CycleEngineException : Exception
    {
        protected CycleEngineException(string message) 
            : base($"\n[CycleEngine]\n{message}") 
        { }
    }
    
    /// <summary>
    /// The engine cannot interpret the syntax of the script.
    /// This happens when Cycle Engine Script were written incorrectly.
    /// </summary>
    public class CycleSyntaxException : CycleEngineException
    {
        public CycleSyntaxException(string details) 
            : base($"Failed to interpret syntax: {details}") 
        { }
    }
    
    /// <summary>
    /// The engine cannot found the resource that it were asked to.
    /// </summary>
    public class CycleResourceNotFoundException : CycleEngineException
    {
        public CycleResourceNotFoundException(string key) 
            : base($"Cannot find resource: {key}") 
        { }
    }
    
    public class CycleServiceNotFoundException : CycleEngineException
    {
        public CycleServiceNotFoundException(string serviceName) 
            : base($"Cannot find service: {serviceName}") 
        { }
    }

    public class CycleEntityNotFoundException : CycleEngineException
    {
        public CycleEntityNotFoundException(string entityKey) 
            : base($"Cannot find entity: {entityKey}")
        { }
    }
    
    /// <summary>
    /// The value provided to the key of the attribute does not match the type that the key requires.
    /// </summary>
    public class CycleAttributeValueTypeMismatchException : CycleEngineException
    {
        public CycleAttributeValueTypeMismatchException(string key, string value) 
            : base($"The value ({value}) of the attribute do not match with it's key ({key})")
        { }
    }
}