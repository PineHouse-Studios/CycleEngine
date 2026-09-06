namespace CycleEngine.Interpreter
{
    public struct Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public int StartIndex { get; set; }
    }

    public enum TokenType
    {
        Directive, Identifier, SectionOrArray,
        Question, DoubleQuestion, Comma, DoubleColon, Colon, LBrace, RBrace, LParentheses, RParentheses, DollarIdentifier,
        String, Number,
        Assign, Operator, Compare, 
        EndOfLine
    }
}