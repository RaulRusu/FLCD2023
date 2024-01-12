using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers.Grammar
{
    public class GeneralProductionRules
    {
        public List<GeneralProduction> Productions = new List<GeneralProduction>();
        public Dictionary<ProductionPart, GeneralProduction> ProductionMap = new Dictionary<ProductionPart, GeneralProduction>();

        public static GeneralProductionRules Empty => newEmpty();

        private static GeneralProductionRules newEmpty()
        {
            return new GeneralProductionRules();
        }

        public static GeneralProductionRules FromFileStream(StreamReader? fileStream)
        {
            var productionRules = new GeneralProductionRules();
            
            var parser = new ProductionParser();
            if (fileStream == null)
                throw new ArgumentNullException(nameof(fileStream));

            while (!fileStream.EndOfStream)
            {
                var line = fileStream.ReadLine();
                if (line == "")
                    continue;
                var productionLine = parser.ParseProductionString(line);
                productionRules.Productions.AddRange(productionLine);
            }

            return productionRules;
        }
    }
}
