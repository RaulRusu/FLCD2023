using FunCompiler.DataStructers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.Structure
{
    public class PIF
    {
        private List<KeyValuePair<string, HashTablePosition>> tokenDictionary = new List<KeyValuePair<string, HashTablePosition>>();

        public void Add(string token, HashTablePosition postion)
        {
            tokenDictionary.Add(new KeyValuePair<string, HashTablePosition>(token, postion));
        }

        public override string ToString()
        {
            var str = "";

            tokenDictionary.ForEach(pair =>
            {
                if (pair.Value == null)
                    str += pair.Key + " " + "null" + "\n";
                else
                    str += pair.Key + " " + pair.Value.ToString() + "\n";
            });

            return str;
        }
    }
}
