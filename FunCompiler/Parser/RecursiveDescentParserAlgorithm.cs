using FunCompiler.DataStructers.Grammar;
using FunCompiler.DataStructers.Grammar.ContextFreeGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FunCompiler.Parser
{
    public class WorkingStackItem
    {
        public string Type = "";
        public int Index = 0;
        public string Value = "";
        public ProductionString RefToCause;

        public WorkingStackItem(string type, int index, string value, ProductionString reference)
        {
            Type = type; 
            Index = index;
            Value = value;
            RefToCause = reference;
        }

        public WorkingStackItem(string type, int index, ProductionString reference)
        {
            Type = type;
            Index = index;
            RefToCause = reference;
        }
    }

    public class RecursiveDescentParserAlgorithm
    {
        private Grammar grammar;
        private Stack<WorkingStackItem> workingStack = new Stack<WorkingStackItem>();
        private Stack<ProductionString> inputStack = new Stack<ProductionString>();
        private string state = "Q";
        private int currentInputIndex = 0;
        private List<String> input { get; set; } = new List<string>();

        public RecursiveDescentParserAlgorithm(Grammar grammar)
        {
            this.grammar = grammar;
        }

        public void PrintState()
        {
            var str = "";

            str += state + ", " + currentInputIndex.ToString() + ", ";
            foreach (var item in workingStack)
            {
                str += item.RefToCause.Value.ToString();
                if (item.Type != "input")
                {
                    str += item.Index.ToString();
                }
            }
            str += ", ";

            foreach (var item in inputStack)
            {
                str += item.Value.ToString();
            }
            Console.WriteLine(str);
        }

        private void Expand()
        {
            Console.WriteLine("expand");
            var prodString = inputStack.Pop();

            var workingStackItem = new WorkingStackItem("prod", 0, prodString);

            workingStack.Push(workingStackItem);

            var production = grammar.Productions.ProductionMap[prodString.Value][0];

            foreach (var symbol in production.Value.AsEnumerable().Reverse())
            {
                inputStack.Push(symbol);
            }
            PrintState();
        }

        private void Advance()
        {
            Console.WriteLine("adv");
            var symbol = inputStack.Pop();
            var workingStackItem = new WorkingStackItem("input", currentInputIndex, input[currentInputIndex], symbol);
            currentInputIndex++;

            workingStack.Push(workingStackItem);
            PrintState();
        }

        private void MomentaryIsuccess()
        {
            Console.WriteLine("mi");
            state = "B";
            PrintState();
        }

        private void Back()
        {
            Console.WriteLine("back");
            var workingStackItem = workingStack.Pop();
            currentInputIndex--;
            inputStack.Push(workingStackItem.RefToCause);
            PrintState();
        }

        private void AnotherTry()
        {
            Console.WriteLine("at");
            var workingStackItem = workingStack.Pop();

            var nonTerminalSymbol = workingStackItem.RefToCause;

            var productionsForNonTerminalSymbol = grammar.Productions.ProductionMap[nonTerminalSymbol.Value];

            if (workingStackItem.Index < productionsForNonTerminalSymbol.Count - 1)
            {
                //clear input stack of old values
                var oldRhs = productionsForNonTerminalSymbol[workingStackItem.Index];

                var counter = oldRhs.Value.Count();

                while(counter > 0)
                {
                    inputStack.Pop();
                    counter--;
                }

                var newIndex = workingStackItem.Index + 1;
                var rhs = productionsForNonTerminalSymbol[newIndex];

                var newWorkingStackItem = new WorkingStackItem("prod", newIndex, nonTerminalSymbol);

                foreach (var symbol in rhs.Value.AsEnumerable().Reverse())
                {
                    inputStack.Push(symbol);
                }

                workingStack.Push(newWorkingStackItem);

                state = "Q";
            }
            else if (currentInputIndex != 0 && !(workingStack.Count == 0 && nonTerminalSymbol.Value == grammar.StartSymbol))
            {
                var oldRhs = productionsForNonTerminalSymbol[workingStackItem.Index];

                var counter = oldRhs.Value.Count();

                while (counter > 0)
                {
                    inputStack.Pop();
                    counter--;
                }

                inputStack.Push(nonTerminalSymbol);
                state = "B";
            }
            else
            {
                var oldRhs = productionsForNonTerminalSymbol[workingStackItem.Index];

                var counter = oldRhs.Value.Count();

                while (counter > 0)
                {
                    inputStack.Pop();
                    counter--;
                }

                state = "E";
            }

            PrintState();
        }

        public void Success()
        {
            state = "F";
        }

        public ParserOutput Parse(string input)
        {
            foreach (var item in input)
            {
                this.input.Add(item.ToString());
            }

            return Parse(this.input);
        }


        public ParserOutput Parse(List<string> input)
        {
            this.input = input;

            state = "Q";
            inputStack.Push(new ProductionString() { Value = grammar.StartSymbol, Type = SymbolType.NonTerminal });

            var n = this.input.Count();

            PrintState();
            while (state != "E" && state != "F")
            {
                if (state == "Q")
                {
                    if (currentInputIndex == n && inputStack.Count == 0) 
                    {
                        Success();
                        continue;
                    }
                    var headOfInputStack = inputStack.Peek();
                    if (headOfInputStack.Type == SymbolType.NonTerminal)
                    {
                        Expand();
                    }
                    else if (currentInputIndex < n && headOfInputStack.Value == this.input[currentInputIndex])
                    {
                        Advance();
                    }
                    else
                    {
                        MomentaryIsuccess();
                    }
                }
                else if (state == "B")
                {
                    var headOfWorkingStack = workingStack.Peek();
                     
                    if (headOfWorkingStack.Type == "input")
                    {
                        Back();
                    }
                    else
                    {
                        AnotherTry();
                    }
                }
            }
            var parserOutput = new ParserOutput();
            if (state == "F")
            {
                parserOutput = ParserOutput.FromRDPWorkingStack(workingStack, grammar);
            }

            return parserOutput;
        }
    }
}
