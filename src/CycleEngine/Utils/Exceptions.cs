using CycleEngine.Core;

namespace CycleEngine.Utils
{
    using System;
    
    public class CycleEngineException : Exception
    {
        protected CycleEngineException(string message) 
            : base($"\n[CycleEngine]\nFile: {ScriptContext.CurrentFile} (Line {ScriptContext.CurrentLineNumber})\nCode: {ScriptContext.CurrentLineText}\n{message}") 
        { }
    }
    
    public class CycleSyntaxException : CycleEngineException
    {
        public CycleSyntaxException(string details) 
            : base($"Failed to interpret syntax: {details}") 
        { }
    }
    
    public class CycleResourceNotFoundException : CycleEngineException
    {
        public CycleResourceNotFoundException(string resName) 
            : base($"Cannot find resource: {resName}") 
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
    
    public class CycleAttributeKeyValueUnmatchException : CycleEngineException
    {
        public CycleAttributeKeyValueUnmatchException(string key, string value) 
            : base($"The value ({value}) of the attribute do not match with it's key ({key})")
        { }
    }
}