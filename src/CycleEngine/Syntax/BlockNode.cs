using System.Collections.Generic;

namespace CycleEngine.Syntax;

public class BlockNode : AstNode
{
    public required IReadOnlyList<AstNode> Child { get; init; }
}