namespace CycleEngine.Interpreter
{
    public class Token
    {
        
    }

    public enum TokenType
    {
        Directive, Identifier, SectionCode,
        Question, Comma, DoubleColon, Colon, LBrace, RBrace, LBracket, RBracket, DollarOpen, DollarClose,
        String, Number,
        Equals, Operator, Compare, 
        EOF
    }
}