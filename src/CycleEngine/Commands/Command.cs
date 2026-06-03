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
        public Command(List<Token> parameters)
        {
            Parameter = parameters;
        }
        
        protected int LineNumber { get; set; }
        protected List<Token> Parameter { get; }
        
        public abstract Task AsyncExecute();
        public abstract void ForceComplete();
    }

    public class CommandBuilder
    {
        public static Command? Build(List<Token> parameters)
        {
            switch (parameters[0].Value)
            {
                case "audio":
                    return new AudioCommand(parameters);
                case "bg":
                    return new BackgroundCommand(parameters);
                case "camera":
                    return new CameraCommand(parameters);
                case "define":
                    throw new NotImplementedException();
                case "dialog":
                    return new DialogCommand(parameters);
                case "function":
                    return new FunctionCommand(parameters);
                case "image":
                    return new ImageCommand(parameters);
                case "jump":
                    return new JumpCommand(parameters);
                case "loadscene":
                    return new LoadSceneCommand(parameters);
                case "loadscript":
                    return new LoadScriptCommand(parameters);
                case "music":
                    return new MusicCommand(parameters);
                case "quit":
                    return new QuitCommand(parameters);
                case "text":
                    return new TextCommand(parameters);
                case "ui":
                    return new UserInterfaceCommand(parameters);
                case "var":
                    return new VarCommand(parameters);
                case "video":
                    return new VideoCommand(parameters);
                case "wait":
                    return new WaitCommand(parameters);
            }

            return null;
        }

        public static CommandBlock? Build(List<Token> parameters, Command[] body)
        {
            switch (parameters[0].Value)
            {
                case "choice":
                    return new ChoiceCommand(parameters, body.ToArray());
                case "if":
                    return new IfCommand(parameters, body.ToArray());
                case "elseif":
                    return new ElseIfCommand(parameters, body.ToArray());
                case "else":
                    return new ElseCommand(parameters, body.ToArray());
                case "repeat":
                    return new RepeatCommand(parameters, body.ToArray());
                case "define":
                    throw new NotImplementedException();
                case "switch":
                    return new SwitchCommand(parameters, body.ToArray());
                case "case":
                    return new CaseCommand(parameters, body.ToArray());
            }

            return null;
        }
    }
}