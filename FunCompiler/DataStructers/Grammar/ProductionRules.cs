using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers.Grammar
{
    public class ProductionRules
    {
        public List<Production> Productions = new List<Production>();
        public Dictionary<ProductionPart, Production> ProductionMap = new Dictionary<ProductionPart, Production>();

        public static ProductionRules Empty => newEmpty();

        private static ProductionRules newEmpty()
        {
            return new ProductionRules();
        }

        public static ProductionRules FromFileStream(StreamReader? fileStream)
        {
            var productionRules = new ProductionRules();
            
            var parser = new ProductionParser();
            if (fileStream == null)
                throw new ArgumentNullException(nameof(fileStream));

            while (fileStream.EndOfStream)
            {
                var line = fileStream.ReadLine();
                var productionLine = parser.ParseProductionString(line);
                productionRules.Productions.AddRange(productionLine);
            }

            return productionRules;
        }
    }
}
