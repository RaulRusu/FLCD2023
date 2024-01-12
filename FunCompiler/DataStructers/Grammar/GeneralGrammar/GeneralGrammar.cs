using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers.Grammar
{
    public class GeneralGrammar
    {
        protected GeneralGrammar() { }

        public List<string> NonTerminals { get; protected set; } = new List<string>();
        public List<string> Terminals { get; protected set; } = new List<string>();
        public string StartSymbol { get; protected set; } = string.Empty;

        public GeneralProductionRules Productions { get; protected set; } = GeneralProductionRules.Empty;

        public bool IsCFG()
        {
            foreach (var prod in Productions.Productions)
            {
                foreach (var symbol in prod.LeftSide.Value)
                {
                    if (symbol.Type == SymbolType.Terminal)
                    {
                        return false;
                    }
                }

                if (prod.LeftSide.Value.Count != 1)
                {
                    return false;
                }
            }

            return true;
        }

        private static IEnumerable<ProductionString> GetAllSymbols(GeneralGrammar grammar)
        {
            foreach (var prod in grammar.Productions.Productions)
            {
                foreach (var symbol in prod.LeftSide.Value)
                {
                    yield return symbol;
                }
                foreach (var symbol in prod.RightSide.Value)
                {
                    yield return symbol;
                }
            }
        }

        public static GeneralGrammar FromFile(string fileName)
        {
            using var fileStream = new StreamReader(fileName);

            var grammar = new GeneralGrammar();

            var line = fileStream.ReadLine();
            grammar.StartSymbol = line;

            grammar.Productions = GeneralProductionRules.FromFileStream(fileStream);

            var nonTermianlSet = new SortedSet<string>();

            var allSymbols = GetAllSymbols(grammar).ToList();

            var allSymbolsSet = new HashSet<ProductionString>();
            allSymbols.ForEach(symbol => allSymbolsSet.Add(symbol));

            var listRef = grammar.Terminals;

            foreach (var symbol in allSymbolsSet)
            {
                if (symbol.Type == SymbolType.Terminal)
                {
                    listRef = grammar.Terminals;
                }
                if (symbol.Type == SymbolType.NonTerminal)
                {
                    listRef = grammar.NonTerminals;
                }
                listRef.Add(symbol.Value);
            }

            return grammar;
        }

        private static List<string> SimpleLineToList(string str, string separators)
        {
            return str
                .Split(separators)
                .Select(x => x.Trim())
                .ToList();
        }
    }
}
