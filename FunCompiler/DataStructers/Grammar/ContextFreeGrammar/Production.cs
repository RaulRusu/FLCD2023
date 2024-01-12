using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers.Grammar.ContextFreeGrammar
{
    public class Production
    {
        public string LeftSide { get; set; } = string.Empty;
        public ProductionPart RightSide { get; set; } = new ProductionPart();

        public override string ToString()
        {
            return $"{LeftSide} -> {RightSide}";
        }
    }
}
