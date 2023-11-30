using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers.Grammar
{
    public enum SymbolType
    {
        Terminal = 0,
        NonTerminal = 1,
        Unkown = 2
    }

    public class ProductionString
    {
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
    }

    public class ProductionPart
    {
        public List<ProductionString> Value = new List<ProductionString>();

        public static ProductionPart FromTupleList(List<(string, int)> values)
        {
            return new ProductionPart()
            {
                Value = values.Select(value => ProductionString.FromTuple(value)).ToList()
            };
            
        }
    }

    public class Production
    {
        public ProductionPart LeftSide { get; set; } = new ProductionPart();
        public ProductionPart RightSide { get; set; } = new ProductionPart();
    }
}
