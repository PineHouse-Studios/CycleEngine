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
            : base($"Failed to interpret: {details}") 
        { }
    }
    
    /// <summary>
    /// The engine cannot execute or understand this command.
    /// This happens when command was failed to execute, or
    /// the command do not exist.
    /// </summary>
    public class CycleCommandException : CycleEngineException
    {
        public CycleCommandException(string details) : base(details)
        { }
    }
    
    /// <summary>
    /// The engine cannot found the file that it were asked to.
    /// </summary>
    public class CycleFileNotFoundException : CycleEngineException
    {
        public CycleFileNotFoundException(string path) 
            : base($"Cannot find file: {path}") 
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
    public class CycleTypeMismatchException : CycleEngineException
    {
        public CycleTypeMismatchException(string key, string value) 
            : base($"The value ({value}) of the attribute do not match with it's type ({key})")
        { }
    }
}