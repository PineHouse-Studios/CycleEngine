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
    public class CycleSyntaxException(string details)
        : CycleEngineException($"Failed to interpret: {details}") { }
    
    /// <summary>
    /// The engine cannot execute or understand this command.
    /// This happens when command was failed to execute, or
    /// the command do not exist.
    /// </summary>
    public class CycleCommandException(string details)
        : CycleEngineException(details) { }
    
    /// <summary>
    /// The engine cannot found the file that it were asked to.
    /// </summary>
    public class CycleFileNotFoundException(string path)
        : CycleEngineException($"Cannot find file: {path}") { }

    /// <summary>
    /// The engine cannot found the resource that it were asked to.
    /// </summary>
    public class CycleResourceNotFoundException(string key)
        : CycleEngineException($"Cannot find resource: {key}") { }
    
    public class CycleServiceNotFoundException(string serviceName)
        : CycleEngineException($"Cannot find service: {serviceName}") { }

    public class CycleEntityNotFoundException(string entityKey)
        : CycleEngineException($"Cannot find entity: {entityKey}") { }
    
    /// <summary>
    /// The value provided to the key of the attribute does not match the type that the key requires.
    /// </summary>
    public class CycleTypeMismatchException(string key, string value)
        : CycleEngineException($"The value ({value}) of the attribute do not match with it's type ({key})") { }

    public class CycleUnexpectedTokenException : CycleCommandException
    {
        public int LineCount;
        public int PosInLineCount;
        public string Token;
        public string CommandSource;

        public CycleUnexpectedTokenException(int posInLineCount, string token)
            : base("")
        {
            LineCount = -1;
            PosInLineCount = posInLineCount;
            Token = token;
            CommandSource = String.Empty;
        }

        public CycleUnexpectedTokenException(CycleUnexpectedTokenException e, string message)
            : base(message)
        {
            LineCount = e.LineCount;
            PosInLineCount = e.PosInLineCount;
            Token = e.Token;
            CommandSource = e.CommandSource;
        }
    }
    
    public class CycleExpectedTokenException : CycleCommandException
    {
        public int LineCount;
        public string Token;
        public string CommandSource;

        public CycleExpectedTokenException(string token)
            : base("")
        {
            LineCount = -1;
            Token = token;
            CommandSource = String.Empty;
        }

        public CycleExpectedTokenException(CycleExpectedTokenException e, string message)
            : base(message)
        {
            LineCount = e.LineCount;
            Token = e.Token;
            CommandSource = e.CommandSource;
        }
    }
}