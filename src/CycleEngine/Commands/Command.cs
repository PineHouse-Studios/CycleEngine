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
        protected CycleEngine _engine;
        public Command(CycleEngine engine, List<Token> parameters, int lineNumber)
        {
            _engine = engine;
            Parameter = parameters;
        }
        
        public int LineNumber { get; private set; }
        protected List<Token> Parameter { get; }
        
        public abstract Task AsyncExecute();
        public abstract void ForceComplete();
    }

    public class CommandBuilder
    {
        public static Command? Build(CycleEngine engine, List<Token> parameters, int lineNumber)
        {
            switch (parameters[0].Value)
            {
                case "audio":
                    return new AudioCommand(engine, parameters, lineNumber);
                case "bg":
                    return new BackgroundCommand(engine, parameters, lineNumber);
                case "camera":
                    return new CameraCommand(engine, parameters, lineNumber);
                case "define":
                    throw new NotImplementedException();
                case "dialog":
                    return new DialogCommand(engine, parameters, lineNumber);
                case "function":
                    return new FunctionCommand(engine, parameters, lineNumber);
                case "image":
                    return new ImageCommand(engine, parameters, lineNumber);
                case "jump":
                    return new JumpCommand(engine, parameters, lineNumber);
                case "loadscene":
                    return new LoadSceneCommand(engine, parameters, lineNumber);
                case "loadscript":
                    return new LoadScriptCommand(engine, parameters, lineNumber);
                case "music":
                    return new MusicCommand(engine, parameters, lineNumber);
                case "quit":
                    return new QuitCommand(engine, parameters, lineNumber);
                case "text":
                    return new TextCommand(engine, parameters, lineNumber);
                case "ui":
                    return new UserInterfaceCommand(engine, parameters, lineNumber);
                case "var":
                    return new VariableCommand(engine, parameters, lineNumber);
                case "video":
                    return new VideoCommand(engine, parameters, lineNumber);
                case "wait":
                    return new WaitCommand(engine, parameters, lineNumber);
            }

            return null;
        }

        public static CommandBlock? Build(CycleEngine engine, List<Token> parameters, Command[] body, int lineNumber)
        {
            switch (parameters[0].Value)
            {
                case "choice":
                    return new ChoiceCommand(engine, parameters, lineNumber, body);
                case "if":
                    return new IfCommand(engine, parameters, lineNumber, body);
                case "elseif":
                    return new ElseIfCommand(engine, parameters, lineNumber, body);
                case "else":
                    return new ElseCommand(engine, parameters, lineNumber, body);
                case "repeat":
                    return new RepeatCommand(engine, parameters, lineNumber, body);
                case "define":
                    throw new NotImplementedException();
                case "switch":
                    return new SwitchCommand(engine, parameters, lineNumber, body);
                case "case":
                    return new CaseCommand(engine, parameters, lineNumber, body);
            }

            return null;
        }
    }
}