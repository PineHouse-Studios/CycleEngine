using System;
using System.Collections.Generic;
using System.Text;
using CycleEngine.Commands;
using CycleEngine.Definitions;
using CycleEngine.Utils;

namespace CycleEngine.Interpreter
{
    public class CycleEngineScriptParser
    {
        private readonly string _source;
        private int _pos;

        // Block commands that have a paired @end... terminator.
        // Add more pairs here as the language grows.
        private static readonly Dictionary<string, string> BlockCommandPairs =
            new(StringComparer.Ordinal)
            {
                { "choice", "endchoice" },
                { "if", "endif" },
                { "elseif", "endelseif"},
                { "else", "endelse" },
                { "repeat", "endrepeat" },
                { "define", "enddefine" },
                { "switch", "endswitch" },
                { "case", "endcase" }
            };
        
        public CycleEngineScriptParser(string source)
        {
            _source = source.ToLower() ?? throw new ArgumentNullException(nameof(source));
            _pos = 0;
        }

        // throws CycleUnexpectedTokenException, CycleExpectedTokenException
        public static CycleEngineScript Parse(string source)
        {
            int curLine = 0;

            string[] lines = source.Split('\n');
            List<Token>[] tokens = new List<Token>[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    tokens[i] = Tokenize(lines[i]);
                }
                catch (Exception e)
                {
                    if (e is CycleUnexpectedTokenException)
                    {
                        CycleUnexpectedTokenException exception = (CycleUnexpectedTokenException)e;
                        exception.LineCount = i + 1;
                        throw exception;
                    }
                    
                    if (e is CycleExpectedTokenException)
                    {
                        CycleExpectedTokenException exception = (CycleExpectedTokenException)e;
                        exception.LineCount = i + 1;
                        throw exception;
                    }
                }
            }

            for (; curLine < lines.Length; curLine++)
            {
                List<Token> cur = tokens[curLine];
                int linePos = 0;
                
                if (cur.Count == 0) continue;

                if (cur[0].Type == TokenType.Directive)
                {
                    
                }

                if (cur[0].Type == TokenType.SectionOrArray)
                {
                    
                }

                for (; linePos < cur.Count; linePos++)
                {
                    
                }



                Token Peek()
                {
                    return cur[linePos + 1];
                }
            }

            List<Token> PeekNextLine()
            {
                return tokens[curLine + 1];
            }
            
            

            return null;
        }
        
        public static List<Token> Tokenize(string line)
        {
            List<Token> tokens = new();
            int pos = 0;

            for (; pos < line.Length; pos++)
            {
                char cur = line[pos];

                // Whitespace
                if (Char.IsWhiteSpace(cur))
                {
                    continue;
                }

                // Directive
                if (cur == '@')
                {
                    if (tokens.Count != 0 || Peek() == Char.MaxValue || Peek() == ' ')
                    {
                        throw new CycleUnexpectedTokenException(pos + 1, cur.ToString());
                    }

                    int peekCount = 1;

                    while (pos + peekCount < line.Length)
                    {
                        char next = line[pos + peekCount];

                        if (next == ' ' || IsTokenBoundary(next))
                        {
                            break;
                        }

                        if (IsIllegalNameCharacter(next))
                        {
                            throw new CycleUnexpectedTokenException(
                                pos + peekCount + 1,
                                next.ToString()
                            );
                        }

                        peekCount++;
                    }

                    tokens.Add(new()
                    {
                        StartIndex = pos,
                        Type = TokenType.Directive,
                        Value = line.Substring(pos, peekCount)
                    });

                    pos += peekCount - 1;
                    continue;
                }

                // Section / Array
                if (cur == '[')
                {
                    if (Peek() == Char.MaxValue)
                    {
                        throw new CycleUnexpectedTokenException(pos + 1, "[");
                    }

                    bool found = false;
                    int peekCount = 2;

                    while (pos + peekCount < line.Length)
                    {
                        char next = line[pos + peekCount];

                        if (next == ']')
                        {
                            found = true;
                            break;
                        }

                        if (next == ' ')
                        {
                            break;
                        }

                        peekCount++;
                    }

                    if (found)
                    {
                        tokens.Add(new()
                        {
                            StartIndex = pos,
                            Type = TokenType.SectionOrArray,
                            Value = line.Substring(pos, peekCount + 1)
                        });

                        pos += peekCount;
                        continue;
                    }

                    throw new CycleExpectedTokenException("]");
                }

                if (cur == ']')
                {
                    throw new CycleUnexpectedTokenException(pos + 1, cur.ToString());
                }

                // Comma
                if (cur == ',')
                {
                    if (tokens.Count == 0)
                    {
                        throw new CycleUnexpectedTokenException(pos + 1, cur.ToString());
                    }

                    tokens.Add(new()
                    {
                        StartIndex = pos,
                        Type = TokenType.Comma,
                        Value = ","
                    });

                    continue;
                }

                // Question
                if (cur == '?')
                {
                    if (tokens.Count == 0)
                    {
                        throw new CycleUnexpectedTokenException(pos + 1, cur.ToString());
                    }

                    if (Peek() == '?')
                    {
                        tokens.Add(new()
                        {
                            StartIndex = pos,
                            Type = TokenType.DoubleQuestion,
                            Value = "??"
                        });

                        pos++;
                        continue;
                    }

                    tokens.Add(new()
                    {
                        StartIndex = pos,
                        Type = TokenType.Question,
                        Value = "?"
                    });

                    continue;
                }

                // Colon
                if (cur == ':')
                {
                    if (tokens.Count == 0)
                    {
                        throw new CycleUnexpectedTokenException(pos + 1, cur.ToString());
                    }

                    if (Peek() == ':')
                    {
                        tokens.Add(new()
                        {
                            StartIndex = pos,
                            Type = TokenType.DoubleColon,
                            Value = "::"
                        });

                        pos++;
                        continue;
                    }

                    tokens.Add(new()
                    {
                        StartIndex = pos,
                        Type = TokenType.Colon,
                        Value = ":"
                    });

                    continue;
                }

                // String
                if (cur == '"')
                {
                    if (Peek() == Char.MaxValue)
                    {
                        throw new CycleUnexpectedTokenException(pos + 1, "\"");
                    }

                    bool found = false;
                    int peekCount = 1;

                    while (pos + peekCount < line.Length)
                    {
                        if (line[pos + peekCount] == '"')
                        {
                            found = true;
                            break;
                        }

                        peekCount++;
                    }

                    if (found)
                    {
                        tokens.Add(new()
                        {
                            StartIndex = pos,
                            Type = TokenType.String,
                            Value = line.Substring(pos, peekCount + 1)
                        });

                        pos += peekCount;
                        continue;
                    }

                    throw new CycleExpectedTokenException("\"");
                }

                // = / ==
                if (cur == '=')
                {
                    if (tokens.Count == 0)
                    {
                        throw new CycleUnexpectedTokenException(pos + 1, cur.ToString());
                    }

                    if (Peek() == '=')
                    {
                        tokens.Add(new()
                        {
                            StartIndex = pos,
                            Type = TokenType.Compare,
                            Value = "=="
                        });

                        pos++;
                        continue;
                    }

                    tokens.Add(new()
                    {
                        StartIndex = pos,
                        Type = TokenType.Assign,
                        Value = "="
                    });

                    continue;
                }

                // > >= < <=
                if (cur is '>' or '<')
                {
                    if (tokens.Count == 0)
                    {
                        throw new CycleUnexpectedTokenException(pos + 1, cur.ToString());
                    }

                    if (Peek() == '=')
                    {
                        tokens.Add(new()
                        {
                            StartIndex = pos,
                            Type = TokenType.Compare,
                            Value = line.Substring(pos, 2)
                        });

                        pos++;
                        continue;
                    }

                    tokens.Add(new()
                    {
                        StartIndex = pos,
                        Type = TokenType.Compare,
                        Value = cur.ToString()
                    });

                    continue;
                }

                // Operators
                if (cur is '+' or '-' or '*' or '/')
                {
                    if (tokens.Count == 0)
                    {
                        throw new CycleUnexpectedTokenException(pos + 1, cur.ToString());
                    }

                    tokens.Add(new()
                    {
                        StartIndex = pos,
                        Type = TokenType.Operator,
                        Value = cur.ToString()
                    });

                    continue;
                }

                // Parentheses
                if (cur == '(')
                {
                    if (tokens.Count == 0)
                    {
                        throw new CycleUnexpectedTokenException(pos + 1, cur.ToString());
                    }

                    tokens.Add(new()
                    {
                        StartIndex = pos,
                        Type = TokenType.LParentheses,
                        Value = cur.ToString()
                    });

                    continue;
                }

                if (cur == ')')
                {
                    if (tokens.Count == 0)
                    {
                        throw new CycleUnexpectedTokenException(pos + 1, cur.ToString());
                    }

                    tokens.Add(new()
                    {
                        StartIndex = pos,
                        Type = TokenType.RParentheses,
                        Value = cur.ToString()
                    });

                    continue;
                }

                // Braces
                if (cur == '{')
                {
                    if (tokens.Count == 0)
                    {
                        throw new CycleUnexpectedTokenException(pos + 1, cur.ToString());
                    }

                    tokens.Add(new()
                    {
                        StartIndex = pos,
                        Type = TokenType.LBrace,
                        Value = cur.ToString()
                    });

                    continue;
                }

                if (cur == '}')
                {
                    if (tokens.Count == 0)
                    {
                        throw new CycleUnexpectedTokenException(pos + 1, cur.ToString());
                    }

                    tokens.Add(new()
                    {
                        StartIndex = pos,
                        Type = TokenType.RBrace,
                        Value = cur.ToString()
                    });

                    continue;
                }

                // Number / Identifier beginning with a number
                if (IsNumber(cur))
                {
                    int peekCount = 1;

                    bool isIdentifier = false;
                    bool isDecimal = false;

                    while (pos + peekCount < line.Length)
                    {
                        char next = line[pos + peekCount];

                        // End of current token
                        if (IsTokenBoundary(next))
                        {
                            break;
                        }

                        if (IsIllegalNameCharacter(next))
                        {
                            throw new CycleUnexpectedTokenException(
                                pos + peekCount + 1,
                                next.ToString()
                            );
                        }

                        if (next == '.')
                        {
                            if (isDecimal)
                            {
                                throw new CycleUnexpectedTokenException(
                                    pos + peekCount + 1,
                                    next.ToString()
                                );
                            }

                            isDecimal = true;
                        }
                        else if (!IsNumber(next))
                        {
                            isIdentifier = true;
                            break;
                        }

                        peekCount++;
                    }

                    if (!isIdentifier)
                    {
                        tokens.Add(new()
                        {
                            StartIndex = pos,
                            Type = TokenType.Number,
                            Value = line.Substring(pos, peekCount)
                        });

                        pos += peekCount - 1;
                        continue;
                    }
                }

                // Identifier / DollarIdentifier
                if (IsIllegalNameCharacter(cur))
                {
                    throw new CycleUnexpectedTokenException(pos + 1, cur.ToString());
                }

                int idPeekCount = 1;
                int dollarCount = cur == '$' ? 1 : 0;

                while (pos + idPeekCount < line.Length)
                {
                    char next = line[pos + idPeekCount];

                    if (IsTokenBoundary(next))
                    {
                        break;
                    }

                    if (IsIllegalNameCharacter(next))
                    {
                        throw new CycleUnexpectedTokenException(
                            pos + idPeekCount + 1,
                            next.ToString()
                        );
                    }

                    if (next == '$')
                    {
                        dollarCount++;
                    }

                    idPeekCount++;
                }
                
                if (dollarCount > 0 && dollarCount % 2 != 0)
                {
                    throw new CycleExpectedTokenException("$");
                }

                tokens.Add(new()
                {
                    StartIndex = pos,
                    Type = dollarCount >= 2
                        ? TokenType.DollarIdentifier
                        : TokenType.Identifier,
                    Value = line.Substring(pos, idPeekCount)
                });

                pos += idPeekCount - 1;
            }

            tokens.Add(new()
            {
                StartIndex = pos,
                Type = TokenType.EndOfLine,
                Value = String.Empty
            });

            return tokens;


            static bool IsNumber(char cur) => cur is >= '0' and <= '9';

            static bool IsIllegalNameCharacter(char cur) => cur is not (
                >= 'a' and <= 'z' or
                >= 'A' and <= 'Z' or
                >= '0' and <= '9' or
                '_' or '.' or '$');

            static bool IsTokenBoundary(char cur) => cur is
                ' ' or '=' or '>' or '<' or '+' or '-' or '*' or '/' or
                '(' or ')' or '{' or '}' or '[' or ']' or ',' or '?' or ':' or '"' or '@';

            char Peek()
            {
                if (pos + 1 >= line.Length)
                {
                    return Char.MaxValue;
                }

                return line[pos + 1];
            }
        }
    }
}
