namespace CycleEngine.Definitions
{

    public enum TokenType
    {
        Command,
        Identifier,
        Key,
        StringLiteral,
        Expression,
        Undefine
    }

    public struct Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public int StartIndex { get; set; }
    }
}