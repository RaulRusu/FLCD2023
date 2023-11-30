using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers.Grammar
{
    public class Grammar
    {
        private Grammar() { }

        public List<string> NonTerminals { get; private set; } = new List<string>();
        public List<string> Terminals { get; private set; } = new List<string>();
        public string StartSymbol { get; private set; } = string.Empty;

        public ProductionRules Productions { get; private set; } = ProductionRules.Empty;

        public Grammar FromFile(string fileName)
        {
            using var fileStream = new StreamReader(fileName);

            var grammar = new Grammar();

            var line = fileStream.ReadLine();

            NonTerminals = SimpleLineToList(line, " ");

            line = fileStream.ReadLine();
            Terminals = SimpleLineToList(line, " ");

            line = fileStream.ReadLine();
            StartSymbol = line;

            Productions = ProductionRules.FromFileStream(fileStream);

            return grammar;
        }


        private List<string> SimpleLineToList(string str, string separators)
        {
            return str
                .Split(separators)
                .Select(x => x.Trim())
                .ToList();
        }
    }
}
