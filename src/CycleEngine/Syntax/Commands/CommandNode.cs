using System.Collections.Generic;
using CycleEngine.Interpreter;

namespace CycleEngine.Syntax;

public class CommandNode : AstNode
{
    public required string Action { get; init; }
    public required IReadOnlyList<ArgumentNode> Arguments { get; init; }
}