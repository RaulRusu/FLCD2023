using FunCompiler.DataStructers.FA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.DataStructers
{
    public class Transition
    {
        public string StartingState { get; set; } = string.Empty;
        public string EndingState { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;

        public static List<Transition> FromList(List<string> strings)
        {
            var generator = TransitionHelpers.FromListToTransion(strings);

            return generator.ToList();
        }
    }
}
