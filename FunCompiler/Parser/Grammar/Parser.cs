using FunCompiler.DataStructers.Grammar;
using FunCompiler.DataStructers.Grammar.ContextFreeGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.Parser
{
    public class Parser
    {
        public void Parse(string grammarFile, string inputFile, string outputFile)
        {
            var grammar = Grammar.FromGeneralGrammar(GeneralGrammar.FromFile(grammarFile));

            var input = File.ReadAllLines(inputFile)[0];

            var alg = new RecursiveDescentParserAlgorithm(grammar);
            var result = alg.Parse(input);

            using var fileStream = File.Open(outputFile, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fileStream);

            Console.WriteLine(result);
            writer.Write(result);
        }

        public void ParseFromPif(string grammarFile, string inputFile, string outputFile)
        {
            var grammar = Grammar.FromGeneralGrammar(GeneralGrammar.FromFile(grammarFile));

            var inputArray = File.ReadAllLines(inputFile);

            var input = inputArray.ToList().Select(line => line.Split(" ")[0]).ToList();

            var alg = new RecursiveDescentParserAlgorithm(grammar);
            var result = alg.Parse(input);

            using var fileStream = File.Open(outputFile, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fileStream);

            Console.WriteLine(result);
            writer.Write(result);
        }
    }
}
