using CycleEngine.Interpreter;

namespace CycleEngine.Syntax;

public abstract class AstNode {
    public Token Token { get; init; }
}