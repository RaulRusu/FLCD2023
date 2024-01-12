using FunCompiler.DataStructers.Grammar.ContextFreeGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FunCompiler.Parser
{
    public class RecursiveDescentParser
    {
        private class RecursiveDescentParserContext
        {
            private Grammar grammar;
            private string input;
            pr
        }

        private Grammar grammar;
        private List<string> basicSeparators = new List<string>
            {
                " ",
                "\t"
            };
        public RecursiveDescentParser(Grammar grammar)
        {
            this.grammar = grammar;
        }

        public Parse(string input)
        {

        }

        private List<string> DeepTokenize(string line)
        {
            var tokenizedLine = new List<string>();
            tokenizedLine = line
                .Split(basicSeparators.ToArray(), StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            tokenizedLine = tokenizedLine
                .Select(part => RegularExpresionSplit(part, @"([;\(\)\[\],])"))
                .SelectMany(element => element)
                .ToList();

            tokenizedLine = tokenizedLine
                .Select(part => RegularExpresionSplit(part, @"(>=|>|<|!=|=|\+|\-|\*|\%|and|or)"))
                .SelectMany(element => element)
                .ToList();
            return tokenizedLine;

        }

        private List<string> RegularExpresionSplit(string input, string pattern)
        {
            var substrings = Regex.Split(input, pattern);
            return substrings
                .Where(part => part != "")
                .ToList();
        }
    }
}
