using FunCompiler.DataStructers;
using FunCompiler.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunCompiler.Lexer
{

    public class Scanner
    {
        private const string ERROR = "ERROR";
        private const string CORRECT = "Lexically correct";

        private List<string> reservedTokens;
        private List<string> separators;
        private List<string> operators;
        private List<string> composedOperators;

        private PIF pif = new PIF();

        private SymbolTable identifiersSymbolTable = new SymbolTable();
        private SymbolTable constantsSymbolTable = new SymbolTable();

        private string LastLogMsg = "";

        public void Init()
        {
            pif = new PIF();
            identifiersSymbolTable = new SymbolTable();
            constantsSymbolTable = new SymbolTable();

            ReadTokens();
        }

        private void ReadTokens()
        {
            var reader = new StreamReader("tokens.in");
            var line = reader.ReadLine();
            separators = line.Split(" ").ToList();
            separators.Add(" ");
            separators.Add("\t");

            line = reader.ReadLine();
            composedOperators = line.Split(" ").ToList();

            line = reader.ReadLine();
            operators = line.Split(" ").ToList();

            line = reader.ReadLine();
            reservedTokens = line.Split(" ").ToList();

            reader.Close();
        }

        public void Scan(string text)
        {
            var lines = text.Split("\r\n");
            var lineIndex = 0;
            string? logMsg = null;
            lines
                .ToList()
                .ForEach(line =>
                {
                    var tokens = DeepTokenize(line);
                    tokens.ForEach(token => Console.Write($"{token} "));
                    Console.Write("\n");
                    if (false)
                    tokens.ForEach(token =>
                    {
                        var result = EvaluateToken(token);

                        if (result == null)
                        {
                            logMsg += "Lexical error: " + "Line - " + lineIndex.ToString() + ", Token - " + token + "\n";
                        }
                    });

                    lineIndex += 1;
                });

            if (logMsg == null)
            {
                LastLogMsg = CORRECT;
            }

            LastLogMsg = logMsg;
        }

        public void Log()
        {
            throw new NotImplementedException();
        }

        private string? EvaluateToken(string token)
        {
            var tokenTypeString = "";
            HashTablePosition tokenPosion = null;
            if (isReserved(token))
            {
                tokenTypeString = token;
                tokenPosion = null;
            }
            else if (IsIdentifier(token))
            {
                tokenTypeString = "0";
                tokenPosion = identifiersSymbolTable.Position(token);
            }
            else
            {
                if (IsInt(token) || IsBool(token) || IsChar(token) || IsFloat(token))
                {
                    tokenTypeString = "1";
                    tokenPosion = constantsSymbolTable.Position(token);
                }
                else
                {
                    return ERROR;
                }
            }

            pif.Add(tokenTypeString, tokenPosion);
            return null;
        }

        private bool IsFloat(string token)
        {
            throw new NotImplementedException();
        }

        private bool IsChar(string token)
        {
            throw new NotImplementedException();
        }

        private bool IsBool(string token)
        {
            throw new NotImplementedException();
        }

        private bool IsInt(string token)
        {
            throw new NotImplementedException();
        }

        private bool IsIdentifier(string token)
        {
            throw new NotImplementedException();
        }

        private bool isReserved(string token)
        {
            throw new NotImplementedException();
        }

        private List<String> DeepTokenize(string line)
        {
            return line
                //Split after separators
                .Split(separators.ToArray(), StringSplitOptions.RemoveEmptyEntries)
                //Split after composed operators
                .Select(token => token.Split(composedOperators.ToArray(), StringSplitOptions.RemoveEmptyEntries))
                .SelectMany(tokens => tokens) // flaten the resulting lists
                //Split after simple operators
                .Select(token => token.Split(operators.ToArray(), StringSplitOptions.RemoveEmptyEntries))
                .SelectMany(tokens => tokens) // flaten the resulting lists
                .ToList();

        }
    }
}
