using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers
{
    public class FiniteAutomata
    {
        public List<string> States { get; private set; } = new List<string>();
        public List<string> InputSymbols { get; private set; } = new List<string>();
        public string InitialState { get; private set; } = "";
        public List<string> FinalStates { get; private set; } = new List<string>();
        public List<Transition> TransitionFunction { get; private set; } = new List<Transition>();

        private Dictionary<string, int> StatesToIndexMapping = new Dictionary<string, int>();

        private List<List<NodeSymbolPair>> GraphRepresentation = new List<List<NodeSymbolPair>>();

        private int GetIndexFromState(string state)
        {
            if (StatesToIndexMapping.ContainsKey(state))
                return StatesToIndexMapping[state];
            return -1;
        }

        public void Init()
        {
            var index = 0;
            foreach (var st in States)
            {
                StatesToIndexMapping.Add(st, index++);
                GraphRepresentation.Add(new List<NodeSymbolPair>());
            }

            TransitionFunction.ForEach(transition =>
            {
                var startIndex = GetIndexFromState(transition.StartingState);
                var endIndex = GetIndexFromState(transition.EndingState);
                var node = new NodeSymbolPair()
                {
                    Node = endIndex,
                    Symbol = transition.Symbol
                };
                GraphRepresentation[startIndex].Add(node);
            });

        }

        private List<string> SimpleLineToList(string str, string separators)
        {
            return str
                .Split(separators)
                .Select(x => x.Trim())
                .ToList();
        }

        private List<Transition> TransitionToList(StreamReader stream)
        {
            var transitions = new List<Transition>();
            while (!stream.EndOfStream)
            {
                var line = stream.ReadLine().Split(" ").ToList();
                transitions.AddRange(Transition.FromList(line));
            }

            return transitions;
        }

        public void FromFile(string fileName)
        {
            using (var fileStream = new StreamReader(fileName))
            {
                var statesString = fileStream.ReadLine();
                States = SimpleLineToList(statesString, " ");

                var inputSymbolsString = fileStream.ReadLine();
                InputSymbols = SimpleLineToList(inputSymbolsString, " ");

                var initalStateString = fileStream.ReadLine();
                InitialState = initalStateString;

                var finalStateString = fileStream.ReadLine();
                FinalStates = SimpleLineToList(finalStateString, " ");

                TransitionFunction = TransitionToList(fileStream);
            }
        }

        public bool Accepts(string sequence)
        {
            var dp = new bool[sequence.Length + 1, States.Count];
            var startIndex = GetIndexFromState(InitialState);
            if (startIndex == -1)
                return false;
            dp[0, startIndex] = true;
            for (var i = 0; i < sequence.Length; i++)
            {
                var symbol = sequence[i].ToString();
                for (var j = 0; j < States.Count; j++)
                {
                    if (dp[i, j])
                    {
                        GraphRepresentation[j].ForEach(node =>
                        {
                            if (node.Symbol == symbol)
                            {
                                dp[i + 1, node.Node] = true;
                            }
                        });
                    }
                }
            }

            /*for (var i = 0; i <= sequence.Length; i++)
            {

                for (var j = 0; j < States.Count; j++)
                {
                    Console.Write(dp[i,j].ToString() + " ");
                }
                Console.WriteLine();
            }*/
            
            foreach (var finalState in FinalStates)
            {
                var index = StatesToIndexMapping[finalState];
                if (dp[sequence.Length, index])
                    return true;
            }
            
            return false;
        }
    }
}
