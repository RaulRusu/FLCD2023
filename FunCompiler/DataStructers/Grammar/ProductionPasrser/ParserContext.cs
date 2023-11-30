using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers.Grammar
{
    public class ParserContext
    {
        private class ParserState
        {
            private ParserContext contex;

            public ParserState(ParserContext parserContext)
            {
                contex = parserContext;
            }

            public enum State
            {
                Outside = 0,
                Inside = 1,
                Start = 2,
                End = 3,
                Unkown = 4,
            }

            public State CurrentState { get; private set; } = State.Start;

            public void ToState(State state)
            {
                CurrentState = state;
            }

            public bool IsEnd()
            {
                return CurrentState == State.End;
            }
        }

        public string Input = string.Empty;
        public char EnterTerminal = char.MinValue;
        public char EscapeChar = char.MinValue;
        public Dictionary<char, char> SpecialToNormalMap = new Dictionary<char, char>();

        private int currentIndex = 0;
        private string workingStr = string.Empty;
        private char currentChar = char.MinValue;
        private ParserState state;
        private int type = 0;

        public ParserContext(string input)
        {
            state = new ParserState(this);
            this.Input = input;
        }
         
        public void Reset()
        {
            Input = string.Empty;
        }

        private bool sequenceEqual(string str, int offset, string sequence)
        {
            if (offset + sequence.Length > str.Length)
                return false;

            for (var index = 0; index < sequence.Length; index++)
            {
                if (str[offset + index] != sequence[index])
                    return false;
            }

            return true;
        }

        private bool isWhiteSpace(char ch)
        {
            return ch == ' ';
        }

        private bool isEnterChar(char ch)
        {
            return ch == EnterTerminal;
        }

        private bool isEndChar(char ch)
        {
            return isWhiteSpace(ch) || isEnterChar(ch);
        }

        private bool isEscapeChar(char ch)
        {
            return ch == EscapeChar;
        }

        private bool endOfString => currentIndex >= workingStr.Length;

        private void next()
        {
            currentIndex++;

            if (endOfString)
            {
                state.ToState(ParserState.State.End);
                currentChar = char.MinValue;
                return;
            }

            currentChar = workingStr[currentIndex];
        }

        private void DoUntilInsideSymbol()
        {
            while (!endOfString && isWhiteSpace(currentChar))
            {
                next();
            }

            if (endOfString)
            {
                return;
            }

            type = isEnterChar(currentChar) ? 1 : 0;
            if (type == 1)
            {
                next();
            }
            state.ToState(ParserState.State.Inside);
        }

        private string BuildSymbol()
        {
            var strBuilder = new StringBuilder();
            while (!endOfString && !isEndChar(currentChar))
            {
                if (isEscapeChar(currentChar))
                {
                    next();

                    if (state.IsEnd())
                        throw new Exception($"Escape char at end of {workingStr}");
                }
                strBuilder.Append(currentChar);
                next();
            }

            if (!endOfString)
                state.ToState(ParserState.State.Outside);

            return strBuilder.ToString();
        }

        private void StartParse(string str)
        {
            workingStr = str;
            currentIndex = 0;
            currentChar = str[currentIndex];
            state.ToState(ParserState.State.Start);
        }

        private (string, int) GetNextSymbol()
        {
            DoUntilInsideSymbol();
            var symbol = BuildSymbol();
            return (symbol, type);
        }

        public IEnumerable<(string, int)> SymbolEnumerable(string str)
        {
            StartParse(str);
            while (!state.IsEnd())
            {
                var symbol = GetNextSymbol();
                if (symbol.Item1 == "")
                    yield break;
                yield return symbol;
            }
        }

        public List<string> SplitFirstRegex(string pattern)
        {
            var result = Regex.Split(Input, pattern);

            return result.ToList();
        }
    }
}
