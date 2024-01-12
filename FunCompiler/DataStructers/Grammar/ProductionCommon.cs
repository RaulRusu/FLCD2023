using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers.Grammar
{
    public enum SymbolType
    {
        Terminal = 1,
        NonTerminal = 0,
        Epsilon = 2,
        Unkown = 3
    }

    public class ProductionString
    {
        public static readonly char Epsilon = Convert.ToChar(201);

        public string Value = string.Empty;
        public SymbolType Type = SymbolType.Unkown;

        public static ProductionString FromTuple((string, int) tuple)
        {
            return new ProductionString()
            {
                Value = tuple.Item1,
                Type = (SymbolType)tuple.Item2
            };
        }

        public override string ToString()
        {
            var value = Type == SymbolType.Epsilon ? Epsilon.ToString() : Value;

            var paddingChar = Type == SymbolType.NonTerminal ? "" : "\"";
            return $"{paddingChar}{value}{paddingChar}";
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            var otherProdString = obj as ProductionString;
            return Value == otherProdString.Value;
        }
    }

    public class ProductionPart
    {
        public List<ProductionString> Value = new List<ProductionString>();

        public ProductionPart() { }

        public static ProductionPart FromTupleList(List<(string, int)> values)
        {
            return new ProductionPart()
            {
                Value = values.Select(value => ProductionString.FromTuple(value)).ToList()
            };
        }

        public override string ToString()
        {
            return Value.Select(prodString => $"{prodString} ")
                .Aggregate(
                    new StringBuilder(), 
                    (strBuilder, current) => strBuilder.Append(current),
                    strBuidler => strBuidler.ToString())
                .TrimEnd();
        }
    }
}
