using System.Collections.Generic;

namespace CycleEngine.Syntax;

public class CommandBlockNode : CommandNode
{
    public required IReadOnlyList<CommandNode> Child { get; init; }
}