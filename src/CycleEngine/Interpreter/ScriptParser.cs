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
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                { "choice", "endchoice" },
                { "if", "endif" },
                { "elseif", "endelseif"},
                { "else", "endelse" },
                { "repeat", "endrepeat" },
                { "define", "enddefine" },
                { "switch", "endswitch" }
            };

        public CycleEngineScriptParser(string source)
        {
            _source = source.ToLower() ?? throw new ArgumentNullException(nameof(source));
            _pos = 0;
        }

        public static CycleEngineScript Parse(string source)
        {
            return new CycleEngineScriptParser(source).ParseScript();
        }

        // ------------------------------------------------------------------
        //  Top-level
        // ------------------------------------------------------------------
        public CycleEngineScript ParseScript()
        {
            var script = new CycleEngineScript
            {
                Attributes = new Dictionary<string, string>(StringComparer.Ordinal),
                Body = new Dictionary<string, ScriptBlock>(StringComparer.Ordinal)
            };

            while (true)
            {
                SkipWhitespaceAndBlankLines();
                if (_pos >= _source.Length) break;

                if (Peek() == '[')
                {
                    var (kind, name) = ReadBracketDirective();
                    if (kind == "begin")
                    {
                        // Every top-level [begin ...] is treated uniformly as
                        // a named block. In practice only "script" appears
                        // here, but the parser doesn't enforce that.
                        var rootBlock = MakeEmptyBlock();
                        FillBlock(rootBlock, OpenerKind.Begin, name);
                        script.Body[name] = rootBlock;
                    }
                    else if (kind == "end")
                    {
                        throw new FormatException(
                            $"Stray '[end {name}]' at top level.");
                    }
                    else
                    {
                        throw new FormatException(
                            $"Unexpected directive '[{kind} {name}]' at top level.");
                    }
                }
                else
                {
                    ParseAttribute(script.Attributes);
                }
            }

            return script;
        }

        private static ScriptBlock MakeEmptyBlock() => new ScriptBlock
        {
            NamedBlocks = new Dictionary<string, ScriptBlock>(StringComparer.Ordinal)
        };

        private enum OpenerKind { Begin, Label }

        // ------------------------------------------------------------------
        //  Attributes:  key = "value"
        // ------------------------------------------------------------------
        private void ParseAttribute(Dictionary<string, string> attributes)
        {
            SkipInlineWhitespace();
            var key = ReadIdentifier();
            SkipInlineWhitespace();

            if (_pos >= _source.Length || _source[_pos] != '=')
                throw new FormatException(
                    $"Expected '=' after attribute key '{key}' at position {_pos}.");
            _pos++; // consume '='

            SkipInlineWhitespace();

            string value;
            if (_pos < _source.Length && _source[_pos] == '"')
            {
                value = ReadStringLiteral();
            }
            else
            {
                // Unquoted attribute value: read to end of line.
                var start = _pos;
                while (_pos < _source.Length && _source[_pos] != '\n' && _source[_pos] != '\r')
                    _pos++;
                value = _source.Substring(start, _pos - start).TrimEnd();
            }

            attributes[key] = value;
            SkipToNextLine();
        }

        // ------------------------------------------------------------------
        //  Block filler
        //
        //  Fills `block` with commands and nested named sub-blocks until
        //  this block's stopper appears.  Behaviour depends on what
        //  opened the block:
        //
        //    OpenerKind.Begin  - opened by [begin name]; closes on the
        //                        matching [end name].
        //    OpenerKind.Label  - opened by a [label] like [0000:1:1];
        //                        closes on the next [label] at the same
        //                        level, on an ancestor's [end name], or
        //                        at EOF. The terminating bracket is NOT
        //                        consumed - the caller handles it.
        //
        //  Both syntactic forms produce the same shape: a single
        //  <see cref="ScriptBlock"/> with its own Commands and NamedBlocks.
        // ------------------------------------------------------------------
        private void FillBlock(ScriptBlock block, OpenerKind opener, string openerName)
        {
            var commands = new List<Command>();

            void Commit() => block.Commands = commands.ToArray();

            while (true)
            {
                SkipWhitespaceAndBlankLines();

                if (_pos >= _source.Length)
                {
                    if (opener == OpenerKind.Begin)
                        throw new FormatException(
                            $"Unexpected EOF inside [begin {openerName}].");
                    Commit();
                    return;
                }

                char c = Peek();

                if (c == '[')
                {
                    int save = _pos;
                    var (kind, dirName) = ReadBracketDirective();

                    if (kind == "end")
                    {
                        if (opener == OpenerKind.Begin && dirName == openerName)
                        {
                            Commit();
                            return;
                        }
                        if (opener == OpenerKind.Label)
                        {
                            // Belongs to an ancestor; rewind for the caller.
                            _pos = save;
                            Commit();
                            return;
                        }
                        throw new FormatException(
                            $"Mismatched '[end {dirName}]' inside " +
                            $"[begin {openerName}] (at position {save}).");
                    }

                    if (kind == "begin")
                    {
                        var child = MakeEmptyBlock();
                        FillBlock(child, OpenerKind.Begin, dirName);
                        block.NamedBlocks[dirName] = child;
                        continue;
                    }

                    // It's a label.
                    string label = kind;

                    if (opener == OpenerKind.Label)
                    {
                        // Same-level label closes our label block; rewind.
                        _pos = save;
                        Commit();
                        return;
                    }

                    // We are a Begin block: open a child label block.
                    var labelChild = MakeEmptyBlock();
                    FillBlock(labelChild, OpenerKind.Label, label);
                    block.NamedBlocks[label] = labelChild;
                    continue;
                }

                if (c == '@')
                {
                    commands.Add(ParseCommandOrBlock());
                    continue;
                }

                throw new FormatException(
                    $"Unexpected character '{c}' at position {_pos} " +
                    $"inside [{(opener == OpenerKind.Begin ? "begin" : "label")} {openerName}].");
            }
        }

        // ------------------------------------------------------------------
        //  Bracket directive reader
        //
        //  Recognised forms:
        //    [begin <name>]
        //    [end <name>]
        //    [<label>]          <- e.g. [0000:1:1]; returned as (label, "")
        //
        //  For begin/end, returns (kind, name).
        //  For labels,    returns (label, "").
        // ------------------------------------------------------------------
        private (string kind, string name) ReadBracketDirective()
        {
            if (_source[_pos] != '[')
                throw new FormatException($"Expected '[' at position {_pos}.");
            _pos++; // consume '['

            // Read the entire bracket content.
            var sb = new StringBuilder();
            while (_pos < _source.Length && _source[_pos] != ']')
            {
                sb.Append(_source[_pos]);
                _pos++;
            }
            if (_pos >= _source.Length)
                throw new FormatException("Unterminated '[' bracket.");
            _pos++; // consume ']'

            var content = sb.ToString().Trim();

            // Match "begin <something>" or "end <something>"
            // Note: the directive keyword is followed by whitespace, but the
            // name itself may contain ':' (e.g. [begin 0000:1]).
            if (TryStripPrefix(content, "begin", out var rest))
                return ("begin", rest.Trim());
            if (TryStripPrefix(content, "end", out rest))
                return ("end", rest.Trim());

            // Otherwise treat the whole thing as a label.
            return (content, string.Empty);
        }

        private static bool TryStripPrefix(string s, string prefix, out string rest)
        {
            if (s.Length > prefix.Length &&
                s.StartsWith(prefix, StringComparison.Ordinal) &&
                char.IsWhiteSpace(s[prefix.Length]))
            {
                rest = s.Substring(prefix.Length);
                return true;
            }
            rest = string.Empty;
            return false;
        }

        // ------------------------------------------------------------------
        //  Command parsing
        //
        //  @name arg1 arg2 ...
        //
        //  If @name is a block-opening command, recursively read body until
        //  the matching @end... command and return a CommandBlock.
        // ------------------------------------------------------------------
        private Command ParseCommandOrBlock()
        {
            int cmdStart = _pos;
            if (_source[_pos] != '@')
                throw new FormatException($"Expected '@' at position {_pos}.");
            _pos++; // consume '@'

            int nameStart = _pos;
            while (_pos < _source.Length && IsIdentifierChar(_source[_pos]))
                _pos++;
            var cmdName = _source.Substring(nameStart, _pos - nameStart);
            if (cmdName.Length == 0)
                throw new FormatException($"Empty command name at position {cmdStart}.");

            var parameters = ParseCommandLineArguments();

            var headToken = new Token
            {
                Type = TokenType.Command,
                Value = cmdName,
                StartIndex = cmdStart
            };

            // Build the parameter list. Convention: the command name itself
            // is the first token so consumers can introspect both name and
            // arguments uniformly.
            var paramList = new List<Token> { headToken };
            paramList.AddRange(parameters);

            // Is this a block-opening command?
            if (BlockCommandPairs.TryGetValue(cmdName, out var endName))
            {
                var bodyCommands = new List<Command>();
                while (true)
                {
                    SkipWhitespaceAndBlankLines();
                    if (_pos >= _source.Length)
                        throw new FormatException(
                            $"Unexpected EOF while looking for '@{endName}'.");

                    if (Peek() == '@')
                    {
                        // Peek the name of the next command to detect the terminator.
                        int save = _pos;
                        _pos++; // skip '@'
                        int ns = _pos;
                        while (_pos < _source.Length && IsIdentifierChar(_source[_pos]))
                            _pos++;
                        var nextName = _source.Substring(ns, _pos - ns);

                        if (nextName == endName)
                        {
                            // Consume the rest of that line (no parameters expected, but be safe).
                            SkipToNextLine();
                            switch (paramList[0].Value)
                            {
                                case "choice":
                                    return new ChoiceCommand(paramList, bodyCommands.ToArray());
                                case "if":
                                    return new IfCommand(paramList, bodyCommands.ToArray());
                                case "elseif":
                                    return new ElseIfCommand(paramList, bodyCommands.ToArray());
                                case "else":
                                    return new ElseCommand(paramList, bodyCommands.ToArray());
                                case "repeat":
                                    return new RepeatCommand(paramList, bodyCommands.ToArray());
                                case "define":
                                    throw new NotImplementedException();
                                case "switch":
                                    return new SwitchCommand(paramList, bodyCommands.ToArray());
                            }
                        }

                        // Not the terminator - rewind and parse normally.
                        _pos = save;
                        bodyCommands.Add(ParseCommandOrBlock());
                    }
                    else if (Peek() == '[')
                    {
                        // A label inside a block command is unusual but tolerate it
                        // by ending the block - however the grammar so far doesn't
                        // mix the two. Treat as an error to surface mistakes.
                        throw new FormatException(
                            $"Unexpected '[' inside @{cmdName} block at position {_pos}.");
                    }
                    else
                    {
                        throw new FormatException(
                            $"Unexpected character '{Peek()}' at position {_pos} " +
                            $"inside @{cmdName} block.");
                    }
                }
            }

            switch (paramList[0].Value)
            {
                case "audio":
                    return new AudioCommand(parameters);
                case "bg":
                    return new BackgroundCommand(parameters);
                case "camera":
                    return new CameraCommand(parameters);
                case "case":
                    return new CaseCommand(parameters);
                case "define":
                    throw new NotImplementedException();
                case "dialog":
                    return new DialogCommand(parameters);
                case "function":
                    return new FunctionCommand(parameters);
                case "image":
                    return new ImageCommand(parameters);
                case "jump":
                    return new JumpCommand(parameters);
                case "loadscene":
                    return new LoadSceneCommand(parameters);
                case "loadscript":
                    return new LoadScriptCommand(parameters);
                case "music":
                    return new MusicCommand(parameters);
                case "quit":
                    return new QuitCommand(parameters);
                case "text":
                    return new TextCommand(parameters);
                case "ui":
                    return new UserInterfaceCommand(parameters);
                case "var":
                    return new VarCommand(parameters);
                case "video":
                    return new VideoCommand(parameters);
                case "wait":
                    return new WaitCommand(parameters);
            }

            throw new CycleCommandException($"Unknown Command: {paramList[0].Value}");
        }

        // ------------------------------------------------------------------
        //  Argument list (rest of the current line after @name)
        // ------------------------------------------------------------------
        private List<Token> ParseCommandLineArguments()
        {
            var tokens = new List<Token>();

            while (true)
            {
                SkipInlineWhitespace();
                if (_pos >= _source.Length) break;
                char c = _source[_pos];
                if (c == '\n' || c == '\r') break;

                tokens.Add(ReadArgumentToken());
            }

            // Consume the line terminator(s).
            SkipToNextLine();
            return tokens;
        }

        // ------------------------------------------------------------------
        //  Single argument token reader.
        //
        //  An argument is a chunk of source. Boundaries:
        //    * Top-level whitespace ends an "atom".
        //    * After an atom, if the next non-space, non-newline char is one
        //      of the whitespace-absorbing binary operators (+ - ? * / = < >),
        //      AND the operator has more content after it on the same line,
        //      then the whitespace, the operator, and the following atom are
        //      all absorbed into the current token. Repeats for chained ops.
        //
        //  This is what lets   sys.lang ? {"zh": 50, "jp": 100}   stay as a
        //  single Expression token while not affecting normal commands like
        //  @bg update bg1 x.velocity:2 dur:3000 .
        // ------------------------------------------------------------------
        private static readonly HashSet<char> BinaryOps =
            new HashSet<char> { '+', '-', '?', '*', '/', '=', '<', '>' };

        private Token ReadArgumentToken()
        {
            int start = _pos;

            // Top-level string literal.
            if (_source[_pos] == '"')
            {
                var s = ReadStringLiteral();
                return new Token
                {
                    Type = TokenType.StringLiteral,
                    Value = s,
                    StartIndex = start
                };
            }

            ScanAtom();

            // Try to absorb `<spaces> <op> <spaces> <more>` chains.
            while (true)
            {
                int i = _pos;
                while (i < _source.Length && (_source[i] == ' ' || _source[i] == '\t'))
                    i++;
                if (i >= _source.Length || _source[i] == '\n' || _source[i] == '\r')
                    break;
                if (!BinaryOps.Contains(_source[i]))
                    break;

                int j = i + 1;
                while (j < _source.Length && (_source[j] == ' ' || _source[j] == '\t'))
                    j++;
                if (j >= _source.Length || _source[j] == '\n' || _source[j] == '\r')
                    break;

                _pos = j;
                ScanAtom();
            }

            var raw = _source.Substring(start, _pos - start);
            return ClassifyArgument(raw, start);
        }

        // Advance _pos over one atom: characters with no top-level whitespace,
        // honouring (), [], {}, "" nesting.
        private void ScanAtom()
        {
            var brackets = new Stack<char>();
            while (_pos < _source.Length)
            {
                char c = _source[_pos];
                if (brackets.Count == 0 &&
                    (c == ' ' || c == '\t' || c == '\n' || c == '\r'))
                    break;

                switch (c)
                {
                    case '(': brackets.Push(')'); break;
                    case '[': brackets.Push(']'); break;
                    case '{': brackets.Push('}'); break;
                    case ')':
                    case ']':
                    case '}':
                        if (brackets.Count > 0 && brackets.Peek() == c)
                            brackets.Pop();
                        break;
                    case '"':
                        _pos++;
                        while (_pos < _source.Length && _source[_pos] != '"')
                        {
                            if (_source[_pos] == '\\' && _pos + 1 < _source.Length) _pos++;
                            _pos++;
                        }
                        break;
                }
                _pos++;
            }
        }

        // ------------------------------------------------------------------
        //  Argument classification
        //
        //  Order:
        //    1. Key         - 'letter.started.id : value'
        //                     (e.g. prog:0, x.velocity:2). The left side of
        //                     ':' must be a classic letter-started identifier.
        //                     This keeps '0000:1' out of Key territory.
        //    2. Identifier  - anything that's a pure value reference (no
        //                     operators / whitespace / quotes at top level):
        //                       bob, sys.lang, 0000:1, MySong, 1000,
        //                       cds[0000:1:1], cds[0000:1:8].option[1],
        //                       $sys.lang$_bob_0000_1_1,
        //                       ChptWhiteStart(chptstart, bg0, bg1)
        //    3. Expression  - operators / whitespace / quotes at top level,
        //                     numeric prefix with a sign, etc.
        //                       sys.lang ? {...}, var+1, -100
        //    4. Undefine    - fallback.
        // ------------------------------------------------------------------
        private static Token ClassifyArgument(string raw, int start)
        {
            int colon = FindTopLevelChar(raw, ':');
            if (colon > 0 && colon < raw.Length - 1 &&
                IsLetterStartedDottedId(raw.Substring(0, colon)))
            {
                return new Token { Type = TokenType.Key, Value = raw, StartIndex = start };
            }

            if (IsIdentifierLike(raw))
            {
                return new Token { Type = TokenType.Identifier, Value = raw, StartIndex = start };
            }

            if (LooksLikeExpression(raw))
            {
                return new Token { Type = TokenType.Expression, Value = raw, StartIndex = start };
            }

            return new Token { Type = TokenType.Undefine, Value = raw, StartIndex = start };
        }

        // Classic letter/underscore-started identifier with optional dots.
        // No leading/trailing dot.  No colons or other separators.
        private static bool IsLetterStartedDottedId(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            if (!(char.IsLetter(s[0]) || s[0] == '_')) return false;
            if (s[s.Length - 1] == '.') return false;
            foreach (var c in s)
            {
                if (!IsIdentifierChar(c) && c != '.') return false;
            }
            return true;
        }

        // True if every character is a "name" character (letters / digits /
        // underscore / '.' / ':').
        private static bool IsNameChar(char c) =>
            char.IsLetterOrDigit(c) || c == '_' || c == '.' || c == ':';

        // Identifier-like: a chain of name chunks, [...] indexers, (...) calls,
        // and $...$ splices.  No top-level operators / whitespace / quotes.
        private static bool IsIdentifierLike(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            if (s[0] == '.' || s[0] == ':') return false;

            int i = 0;
            int n = s.Length;
            bool sawAny = false;

            while (i < n)
            {
                char c = s[i];

                if (IsNameChar(c))
                {
                    while (i < n && IsNameChar(s[i])) i++;
                    sawAny = true;
                    continue;
                }

                if (c == '[')
                {
                    int end = SkipBracket(s, i, '[', ']');
                    if (end < 0) return false;
                    i = end;
                    sawAny = true;
                    continue;
                }

                if (c == '(')
                {
                    int end = SkipBracket(s, i, '(', ')');
                    if (end < 0) return false;
                    i = end;
                    sawAny = true;
                    continue;
                }

                if (c == '$')
                {
                    int j = s.IndexOf('$', i + 1);
                    if (j < 0) return false;
                    i = j + 1;
                    sawAny = true;
                    continue;
                }

                return false;
            }
            return sawAny;
        }

        // s[i] must equal open.  Returns the index just past the matching
        // close, or -1 if unmatched.  Handles nested brackets and skips
        // string literals.
        private static int SkipBracket(string s, int i, char open, char close)
        {
            int depth = 0;
            while (i < s.Length)
            {
                char c = s[i];
                if (c == open)
                {
                    depth++;
                }
                else if (c == close)
                {
                    depth--;
                    if (depth == 0) return i + 1;
                }
                else if (c == '"')
                {
                    i++;
                    while (i < s.Length && s[i] != '"')
                    {
                        if (s[i] == '\\' && i + 1 < s.Length) i++;
                        i++;
                    }
                }
                else if ((c == '(' || c == '[' || c == '{') && c != open)
                {
                    char otherClose = c == '(' ? ')' : (c == '[' ? ']' : '}');
                    int j = SkipBracket(s, i, c, otherClose);
                    if (j < 0) return -1;
                    i = j;
                    continue;
                }
                i++;
            }
            return -1;
        }

        private static bool LooksLikeExpression(string s)
        {
            foreach (var c in s)
            {
                if (c == '$' || c == '?' || c == '(' || c == ')' ||
                    c == '[' || c == ']' || c == '{' || c == '}' ||
                    c == '+' || c == '-' || c == '*' || c == '/' ||
                    c == '<' || c == '>' || c == '=' || c == '!' ||
                    c == ',' || c == '"' || char.IsWhiteSpace(c))
                    return true;
            }
            if (s.Length > 0 && (char.IsDigit(s[0]) || s[0] == '-' || s[0] == '+'))
                return true;
            return false;
        }

        // Find the first top-level occurrence of `target` in `s`
        // (respecting (), [], {}, "" nesting). Returns -1 if not found.
        private static int FindTopLevelChar(string s, char target)
        {
            var stack = new Stack<char>();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (stack.Count == 0 && c == target) return i;
                switch (c)
                {
                    case '(': stack.Push(')'); break;
                    case '[': stack.Push(']'); break;
                    case '{': stack.Push('}'); break;
                    case ')':
                    case ']':
                    case '}':
                        if (stack.Count > 0 && stack.Peek() == c) stack.Pop();
                        break;
                    case '"':
                        i++;
                        while (i < s.Length && s[i] != '"')
                        {
                            if (s[i] == '\\' && i + 1 < s.Length) i++;
                            i++;
                        }
                        break;
                }
            }
            return -1;
        }

        // ------------------------------------------------------------------
        //  Low-level lexing helpers
        // ------------------------------------------------------------------
        private char Peek() => _pos < _source.Length ? _source[_pos] : '\0';

        private static bool IsIdentifierStart(char c) =>
            char.IsLetter(c) || c == '_';

        private static bool IsIdentifierChar(char c) =>
            char.IsLetterOrDigit(c) || c == '_';

        private string ReadIdentifier()
        {
            int start = _pos;
            while (_pos < _source.Length && IsIdentifierChar(_source[_pos]))
                _pos++;
            if (start == _pos)
                throw new FormatException($"Expected identifier at position {start}.");
            return _source.Substring(start, _pos - start);
        }

        private string ReadStringLiteral()
        {
            if (_source[_pos] != '"')
                throw new FormatException($"Expected '\"' at position {_pos}.");
            _pos++; // consume opening quote
            var sb = new StringBuilder();
            while (_pos < _source.Length && _source[_pos] != '"')
            {
                if (_source[_pos] == '\\' && _pos + 1 < _source.Length)
                {
                    char esc = _source[_pos + 1];
                    sb.Append(esc switch
                    {
                        'n' => '\n',
                        'r' => '\r',
                        't' => '\t',
                        '"' => '"',
                        '\\' => '\\',
                        _ => esc
                    });
                    _pos += 2;
                }
                else
                {
                    sb.Append(_source[_pos]);
                    _pos++;
                }
            }
            if (_pos >= _source.Length)
                throw new FormatException("Unterminated string literal.");
            _pos++; // consume closing quote
            return sb.ToString();
        }

        private void SkipInlineWhitespace()
        {
            while (_pos < _source.Length &&
                   (_source[_pos] == ' ' || _source[_pos] == '\t'))
                _pos++;
        }

        private void SkipWhitespaceAndBlankLines()
        {
            while (_pos < _source.Length && char.IsWhiteSpace(_source[_pos]))
                _pos++;
        }

        private void SkipToNextLine()
        {
            while (_pos < _source.Length &&
                   _source[_pos] != '\n' && _source[_pos] != '\r')
                _pos++;
            // Consume \r, \n, or \r\n.
            if (_pos < _source.Length && _source[_pos] == '\r') _pos++;
            if (_pos < _source.Length && _source[_pos] == '\n') _pos++;
        }
    }
}