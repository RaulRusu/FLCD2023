using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
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

    public class BaseProductionPart<T> where T: new()
    {
        public T Value = new T();
    }

    public class BaseProduction<T> where T: new()
    {
        public BaseProductionPart<T> LeftSide { get; set; }
        public BaseProductionPart<T> RightSide { get; set; };
    }
}
*/