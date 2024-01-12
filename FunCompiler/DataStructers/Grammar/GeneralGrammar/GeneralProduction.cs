using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers.Grammar
{
    public class GeneralProduction
    {
        public ProductionPart LeftSide { get; set; } = new ProductionPart();
        public ProductionPart RightSide { get; set; } = new ProductionPart();
    }
}
