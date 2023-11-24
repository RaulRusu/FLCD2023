using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers.FA
{
    public static class TransitionHelpers
    {
        private static Dictionary<string, IEnumerable<string>> patternMap = initDictionary();

        private static IEnumerable<string> GenDigitsInRange(int start, int end)
        {
            for (var number = start; number <= end; number++)
            {
                yield return number.ToString();
            }
        }

        private static IEnumerable<string> GenLettersInRange(string startLetter, string endLetter)
        {
            var startChar = startLetter[0];
            var endChar = endLetter[0];

            for (var letter = startChar; letter <= endChar; letter++)
            {
                yield return letter.ToString();
            }
        }

        private static Dictionary<string, IEnumerable<string>> initDictionary()
        {
            var map = new Dictionary<string, IEnumerable<string>>();

            map.Add("1...9", GenDigitsInRange(1, 9));
            map.Add("0...9", GenDigitsInRange(0, 9));
            map.Add("a...z", GenLettersInRange("a", "z"));
            map.Add("A...Z", GenLettersInRange("A", "Z"));

            return map;
        }

        public static IEnumerable<Transition> FromListToTransion(List<string> strings)
        {
            if (strings.Count < 2)
                yield break;

            var startingState = strings[0];
            var endingState = strings[1];
            var symbolPattern = strings[2];

            if (patternMap.ContainsKey(symbolPattern))
            {
                var generator = patternMap[symbolPattern];
                foreach (var symbol in generator)
                {
                    yield return new Transition
                    {
                        StartingState = startingState,
                        EndingState = endingState,
                        Symbol = symbol
                    };
                }
            }
            else
            {
                yield return new Transition
                {
                    StartingState = startingState,
                    EndingState = endingState,
                    Symbol = symbolPattern
                };
            }
        }
    }
}
