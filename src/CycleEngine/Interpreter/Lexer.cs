using System.Collections.Generic;
using System.Text;
using CycleEngine.Utils;

namespace CycleEngine.Interpreter
{
    public class CommandLexer
    {
        public List<Token> Tokenize(string line)
        {
            var tokens = new List<Token>();
            int pos = 0;

            while (pos < line.Length)
            {
                char c = line[pos];
                
                if (char.IsWhiteSpace(c))
                {
                    pos++;
                    continue;
                }
                
                if (c == '#' && pos + 1 < line.Length && line[pos + 1] == '#') break;
                
                if (c == '"')
                {
                    int start = pos;
                    pos++;
                    StringBuilder sb = new StringBuilder();
                    while (pos < line.Length && line[pos] != '"')
                    {
                        if (line[pos] == '\\' && pos + 1 < line.Length) pos++;
                        sb.Append(line[pos]);
                        pos++;
                    }
                    pos++;
                    tokens.Add(new Token { Type = TokenType.StringLiteral, Value = sb.ToString(), StartIndex = start });
                    continue;
                }
                
                int wordStart = pos;
                while (pos < line.Length && !char.IsWhiteSpace(line[pos]) && line[pos] != '"' && line[pos] != ':')
                {
                    pos++;
                }
                string word = line.Substring(wordStart, pos - wordStart);
                
                if (word.StartsWith("@"))
                {
                    tokens.Add(new Token { Type = TokenType.Command, Value = word, StartIndex = wordStart });
                }
                else
                {
                    int tempPos = pos;
                    while (tempPos < line.Length && char.IsWhiteSpace(line[tempPos])) tempPos++;

                    if (tempPos < line.Length && line[tempPos] == ':')
                    {
                        tokens.Add(new Token { Type = TokenType.Key, Value = word, StartIndex = wordStart });
                        
                        pos = tempPos + 1;
                        
                        while (pos < line.Length && char.IsWhiteSpace(line[pos])) pos++;
                        if (pos < line.Length && line[pos] == '"') continue; 
                        
                        int exprStart = pos;
                        StringBuilder exprBuilder = new StringBuilder();
                        while (pos < line.Length)
                        {
                            int lookaheadPos = pos;
                            while (lookaheadPos < line.Length && char.IsWhiteSpace(line[lookaheadPos])) lookaheadPos++;
                            
                            int nextWordStart = lookaheadPos;
                            while (lookaheadPos < line.Length && !char.IsWhiteSpace(line[lookaheadPos]) && line[lookaheadPos] != '"' && line[lookaheadPos] != ':') lookaheadPos++;
                            
                            int colonCheckPos = lookaheadPos;
                            while (colonCheckPos < line.Length && char.IsWhiteSpace(line[colonCheckPos])) colonCheckPos++;
                            if (colonCheckPos < line.Length && line[colonCheckPos] == ':') break;

                            exprBuilder.Append(line[pos]);
                            pos++;
                        }

                        string finalExpr = exprBuilder.ToString().Trim();
                        if (!string.IsNullOrEmpty(finalExpr))
                        {
                            tokens.Add(new Token { Type = TokenType.Expression, Value = finalExpr, StartIndex = exprStart });
                        }
                    }
                    else
                    {
                        tokens.Add(new Token { Type = TokenType.Identifier, Value = word, StartIndex = wordStart });
                    }
                }
            }

            return tokens;
        }
    }
}