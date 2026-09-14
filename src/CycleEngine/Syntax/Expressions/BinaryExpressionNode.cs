namespace CycleEngine.Syntax;

public sealed class BinaryExpressionNode : AstNode
{
    public required ExpressionNode Left { get; init; }
    public required ExpressionNode Right { get; init; }
}