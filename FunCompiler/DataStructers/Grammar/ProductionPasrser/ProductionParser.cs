using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers.Grammar
{
    public class ProductionParser
    {
        private const string PRODUCTION_SYMBOL_PATTERN = @"(->)";
        private const string PRODUCTINION_OR_PATTERN = @"(?<!\/)\|";

        private ParserContext createContext(string input)
        {
            var parserContext = new ParserContext(input)
            {
                EnterTerminal = '"',
                EscapeChar = '/',
                Input = input,
                SpecialToNormalMap = null
            };

            parserContext.Input = input;

            return parserContext;
        }

        private ProductionPart ParseLeft(ParserContext context, string leftString)
        {
            var left = context.SymbolEnumerable(leftString).ToList();
            var leftProductionPart = ProductionPart.FromTupleList(left);

            return leftProductionPart;
        }
        
        private IEnumerable<ProductionPart> ParseRight(ParserContext context, string rightString)
        {
            var righStringParts = Regex.Split(rightString, PRODUCTINION_OR_PATTERN)
                .Select(str => str.Trim())
                .ToList();

            foreach(var part in righStringParts)
            {
                var right = context.SymbolEnumerable(part).ToList();
                var rightProductionPart = ProductionPart.FromTupleList(right);
                yield return rightProductionPart;
            }
        }

        // abc "a" -> asd "asda asdas `" sadada " //" asd | asd asd asd 
        public List<Production> ParseProductionString(string input)
        {
            var parserContext = createContext(input);

            var splitResult = parserContext.SplitFirstRegex(PRODUCTION_SYMBOL_PATTERN);

            if (splitResult.Count != 3)
                throw new Exception($"invalid production: {input}");

            var lhsString = splitResult[0].Trim();
            var rhsString = splitResult[2].Trim();

            var leftProductionPart = ParseLeft(parserContext, lhsString);

            var rightParts = ParseRight(parserContext, rhsString);
            return rightParts
                .Select(rightProductionPart => new Production
                {
                    LeftSide = leftProductionPart,
                    RightSide = rightProductionPart
                })
                .ToList();
        }
    }
}