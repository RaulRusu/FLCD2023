using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers.Grammar.ContextFreeGrammar
{
    public sealed class Grammar
    {
        public List<string> NonTerminals { get; private set; } = new List<string>();
        public List<string> Terminals { get; private set; } = new List<string>();
        public string StartSymbol { get; private set; } = string.Empty;
        public ProductionRules Productions { get; private set; } = ProductionRules.Empty;

        protected GeneralGrammar baseGrammar;

        protected Grammar() { }

        public static Grammar FromGeneralGrammar(GeneralGrammar generalGrammar)
        {
            if (!generalGrammar.IsCFG())
            {
                throw new Exception("Grammar is not context free");
            }

            var grammar = new Grammar();
            grammar.baseGrammar = generalGrammar;
            grammar.NonTerminals = generalGrammar.NonTerminals;
            grammar.Terminals = generalGrammar.Terminals;
            grammar.StartSymbol = generalGrammar.StartSymbol;
            grammar.Productions = ProductionRules.FromGeneralProdcutionRules(generalGrammar.Productions);

            grammar.Productions.Productions.ForEach(prod => Console.WriteLine(prod));

            return grammar;
        }

        public string GetTerminalString()
        {
            var strBuilder = new StringBuilder();

            Terminals.ForEach(symbol => strBuilder.Append($"{symbol} "));

            return strBuilder.ToString().TrimEnd();
        }

        public string GetNonTerminalString()
        {
            var strBuilder = new StringBuilder();

            NonTerminals.ForEach(symbol => strBuilder.Append($"{symbol} "));

            return strBuilder.ToString().TrimEnd();
        }

        public string GetProductionsString()
        {
            return Productions.ToPrettyProductionString();
        }

        public List<ProductionPart>? GetProductionsFor(string nonTerminalSymbol)
        {
            return Productions.ProductionMap.ContainsKey(nonTerminalSymbol) ? 
                Productions.ProductionMap[nonTerminalSymbol] : null;
        }

        public override string ToString()
        {
            return $"NonTerminal: {GetNonTerminalString()}\nTerminal {GetNonTerminalString()}\n{GetProductionsString()}";
        }
    }
}
