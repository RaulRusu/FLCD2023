using FunCompiler.DataStructers.Grammar;
using FunCompiler.DataStructers.Grammar.ContextFreeGrammar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.Parser
{
    public class ParserOutput
    {
        private List<WorkingStackItem> Productions = new List<WorkingStackItem>();
        private List<List<ProductionString>> DerivationStrings = new List<List<ProductionString>>();
        private Grammar grammar;
        private bool accepted = false; 

        public static ParserOutput FromRDPWorkingStack(Stack<WorkingStackItem> stack, Grammar grammar)
        {
            var parserOutput = new ParserOutput();

            parserOutput.grammar = grammar;
            parserOutput.accepted = true;
            while (stack.Count > 0)
            {
                var stackItem = stack.Pop();

                if (stackItem.Type == "prod")
                {
                    parserOutput.Productions.Add(stackItem);
                }
            }

            var productionStr = new ProductionString()
            {
                Value = parserOutput.grammar.StartSymbol,
                Type = SymbolType.NonTerminal
            };

            var start = new List<ProductionString>() { productionStr };
            parserOutput.Productions.Reverse();
            parserOutput.CreateDerivationStrings(start);
            
            return parserOutput;
        }

        private void CreateDerivationStrings(List<ProductionString> start)
        {
            DerivationStrings.Add(start);

            var last = start;

            Productions.ForEach(prod =>
            {
                var newList = new List<ProductionString>();
                var isFirst = true;
                last.ForEach(ps =>
                {
                    if (ps.Type == SymbolType.NonTerminal)
                    {
                        if (isFirst == true)
                        {
                            var rhs = grammar.Productions.ProductionMap[ps.Value][prod.Index];
                            rhs.Value.ForEach(prod => newList.Add(prod));
                            isFirst = false;
                        }
                        else
                        {
                            newList.Add(ps);
                        }
                    }
                    else
                    {
                        newList.Add(ps);
                    }
                });

                DerivationStrings.Add(newList);
                last = newList;
            });

            
        }

        public override string ToString()
        {
            var str = "";
            if (accepted == true)
            {
                str += "accepted\n";

                DerivationStrings.ForEach(part =>
                {
                    part.ForEach(ds => str += ds.Value);
                    str += "=>";
                });

                str = str.Substring(0, str.Length - 2);
            }
            else
            {
                str += "error\n";
            }

            return str;
        }
    }
}
