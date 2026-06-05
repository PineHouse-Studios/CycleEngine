using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleEngine.Core;
using CycleEngine.Definitions;
using CycleEngine.Interpreter;
using CycleEngine.Utils;

namespace CycleEngine.Commands
{
    public abstract class Command : ICommand
    {
        public Command(List<Token> parameters, int lineNumber)
        {
            Parameter = parameters;
        }
        
        public int LineNumber { get; private set; }
        protected List<Token> Parameter { get; }
        
        public abstract Task AsyncExecute();
        public abstract void ForceComplete();
    }

    public class CommandBuilder
    {
        public static Command? Build(List<Token> parameters, int lineNumber)
        {
            switch (parameters[0].Value)
            {
                case "audio":
                    return new AudioCommand(parameters, lineNumber);
                case "bg":
                    return new BackgroundCommand(parameters, lineNumber);
                case "camera":
                    return new CameraCommand(parameters, lineNumber);
                case "define":
                    throw new NotImplementedException();
                case "dialog":
                    return new DialogCommand(parameters, lineNumber);
                case "function":
                    return new FunctionCommand(parameters, lineNumber);
                case "image":
                    return new ImageCommand(parameters, lineNumber);
                case "jump":
                    return new JumpCommand(parameters, lineNumber);
                case "loadscene":
                    return new LoadSceneCommand(parameters, lineNumber);
                case "loadscript":
                    return new LoadScriptCommand(parameters, lineNumber);
                case "music":
                    return new MusicCommand(parameters, lineNumber);
                case "quit":
                    return new QuitCommand(parameters, lineNumber);
                case "text":
                    return new TextCommand(parameters, lineNumber);
                case "ui":
                    return new UserInterfaceCommand(parameters, lineNumber);
                case "var":
                    return new VarCommand(parameters, lineNumber);
                case "video":
                    return new VideoCommand(parameters, lineNumber);
                case "wait":
                    return new WaitCommand(parameters, lineNumber);
            }

            return null;
        }

        public static CommandBlock? Build(List<Token> parameters, Command[] body, int lineNumber)
        {
            switch (parameters[0].Value)
            {
                case "choice":
                    return new ChoiceCommand(parameters, body.ToArray(), lineNumber);
                case "if":
                    return new IfCommand(parameters, body.ToArray(), lineNumber);
                case "elseif":
                    return new ElseIfCommand(parameters, body.ToArray(), lineNumber);
                case "else":
                    return new ElseCommand(parameters, body.ToArray(), lineNumber);
                case "repeat":
                    return new RepeatCommand(parameters, body.ToArray(), lineNumber);
                case "define":
                    throw new NotImplementedException();
                case "switch":
                    return new SwitchCommand(parameters, body.ToArray(), lineNumber);
                case "case":
                    return new CaseCommand(parameters, body.ToArray(), lineNumber);
            }

            return null;
        }
    }
}