using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers.Grammar.ContextFreeGrammar
{
    public sealed class ProductionRules
    {
        public List<Production> Productions { get; private set; } = new List<Production>();
        public Dictionary<string, List<ProductionPart>> ProductionMap { get; private set; } = new Dictionary<string, List<ProductionPart>>();

        public static ProductionRules Empty => newEmpty();

        private static ProductionRules newEmpty()
        {
            return new ProductionRules();
        }

        public static ProductionRules FromGeneralProdcutionRules(GeneralProductionRules generalProductionRules)
        {
            var productionRules = new ProductionRules();

            generalProductionRules.Productions.ForEach(generalProduction =>
            {
                var production = new Production()
                {
                    LeftSide = generalProduction.LeftSide.Value[0].Value,
                    RightSide = generalProduction.RightSide
                };

                productionRules.Productions.Add(production);
                if (!productionRules.ProductionMap.ContainsKey(production.LeftSide))
                {
                    productionRules.ProductionMap[production.LeftSide] = new List<ProductionPart>();
                }
                productionRules.ProductionMap[production.LeftSide].Add(production.RightSide);
            });

            return productionRules;
        }

        public override string ToString()
        {
            return Productions
                .Select(prod => prod.ToString() + "\n")
                .Aggregate(
                    new StringBuilder(),
                    (strBuilder, current) => strBuilder.Append(current),
                    strBuidler => strBuidler.ToString())
                .TrimEnd();
        }

        public string ToPrettyProductionString()
        {
            char[] endingChars = { '|', ' ' };

            string ProductionPartToString(List<ProductionPart> part) =>
                part
                    .Select(part => part.ToString() + " | ")
                    .Aggregate(
                        new StringBuilder(),
                        (strBuilder, current) => strBuilder.Append(current),
                        strBuidler => strBuidler.ToString())
                    .TrimEnd(endingChars);

            return ProductionMap
                    .Select(pair => $"{pair.Key} -> {ProductionPartToString(pair.Value)}\n")
                    .Aggregate(
                        new StringBuilder(),
                        (strBuilder, current) => strBuilder.Append(current),
                        strBuidler => strBuidler.ToString())
                    .TrimEnd();
        }
    }
}
