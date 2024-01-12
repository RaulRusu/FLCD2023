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

        public string Input { get; set; } = string.Empty;
        public char EnterTerminal { get; set; } = char.MinValue;
        public char EscapeChar { get; set; } = char.MinValue;
        public Dictionary<char, char> SpecialToNormalMap { get; set; } = new Dictionary<char, char>();

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

        private bool IsWhiteSpace(char ch)
        {
            return ch == ' ';
        }

        private bool IsEnterChar(char ch)
        {
            return ch == EnterTerminal;
        }

        private bool IsEndChar(char ch)
        {
            return IsWhiteSpace(ch) || IsEnterChar(ch);
        }

        private bool IsEscapeChar(char ch)
        {
            return ch == EscapeChar;
        }

        private bool IsEndOfSymbol(char ch)
        {
            if (type == 1)
                return IsEnterChar(ch);
            return IsWhiteSpace(ch);
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

        private void SkipEnterChar()
        {
            if (!IsEnterChar(currentChar))
                return;

            next();
        }

        private void DoWhileOutsideSymbol()
        {
            if (state.CurrentState != ParserState.State.Outside)
                return;

            while (!endOfString && IsWhiteSpace(currentChar))
            {
                next();
            }

            if (endOfString)
            {
                return;
            }

            type = IsEnterChar(currentChar) ? 1 : 0;
            if (type == 1)
            {
                next();
            }
            state.ToState(ParserState.State.Inside);
        }

        private string BuildSymbol()
        {
            var strBuilder = new StringBuilder();
            while (!endOfString && !IsEndOfSymbol(currentChar))
            {
                var nextCharToAdd = currentChar;
                if (IsEscapeChar(currentChar))
                {
                    next();

                    if (state.IsEnd())
                        throw new Exception($"Escape char at end of {workingStr}");

                    if (currentChar == 'E')
                    {
                        nextCharToAdd = 'E';
                        type = 2;
                    }
                }
                strBuilder.Append(nextCharToAdd);
                next();
            }

            if (type == 1)
            {
                SkipEnterChar();
            }

            if (!endOfString)
                state.ToState(ParserState.State.Outside);

            return strBuilder.ToString();
        }

        private void SetInitialState()
        {
            if (IsWhiteSpace(currentChar))
            {
                state.ToState(ParserState.State.Outside);
                return;
            }

            type = IsEnterChar(currentChar) ? 1 : 0;
            if (type == 1)
            {
                SkipEnterChar();
            }

            state.ToState(ParserState.State.Inside);
        }

        private void StartParse(string str)
        {
            workingStr = str;
            currentIndex = 0;
            currentChar = str[currentIndex];
            SetInitialState();
        }

        private (string, int) GetNextSymbol()
        {
            DoWhileOutsideSymbol();
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
