using FunCompiler.DataStructers;
using FunCompiler.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FunCompiler.Lexer
{

    public class Scanner
    {
        private const string ERROR = "ERROR";
        private const string CORRECT = "Lexically correct\n";

        private List<string> reservedTokens;
        private List<string> basicSeparators;
        private List<string> separators;
        private List<string> operators;
        private List<string> composedOperators;

        private PIF pif = new PIF();

        private SymbolTable identifiersSymbolTable = new SymbolTable();
        private SymbolTable constantsSymbolTable = new SymbolTable();

        private string LastLogMsg = "";
        private string tonkenString = "";

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
            basicSeparators = new List<string>
            {
                " ",
                "\t"
            };

            line = reader.ReadLine();
            composedOperators = line.Split(" ").ToList();

            line = reader.ReadLine();
            operators = line.Split(" ").ToList();

            line = reader.ReadLine();
            reservedTokens = line.Split(" ").ToList();
            reservedTokens.AddRange(composedOperators);
            reservedTokens.AddRange(operators);
            reservedTokens.AddRange(separators);

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

                    tokens.ForEach(token =>
                    {
                        var result = EvaluateToken(token);

                        if (result == ERROR)
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
            else
            {
                LastLogMsg = logMsg;
            }
        }

        public void Log()
        {
            using var fileStream = File.Open("output.log", FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fileStream);
            writer.WriteLine(LastLogMsg);

            writer.WriteLine("PIF");
            writer.WriteLine(pif);

            writer.WriteLine("Identifiers Symbol Table");
            writer.WriteLine(identifiersSymbolTable);

            writer.WriteLine("Constants Symbol Table");
            writer.WriteLine(constantsSymbolTable);
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

        //^[-+]?[0-9]+\.[0-9]+$
        private bool IsFloat(string token)
        {
            var match = Regex.Match(token, @"^[-+]?[0-9]+\.[0-9]+$");
            return match.Success;
        }

        //^[a-zA-Z]$
        private bool IsChar(string token)
        {
            var match = Regex.Match(token, @"^[0-9a-zA-Z]$");
            return match.Success;
        }

        private bool IsBool(string token)
        {
            return token == "true" || token == "false";
        }
        //^[-+]?[0-9]+$
        private bool IsInt(string token)
        {
            var match = Regex.Match(token, @"^[-+]?[0-9]+$");
            return match.Success;
            
        }

        //^[a-zA-Z][a-zA-Z0-9]*$
        private bool IsIdentifier(string token)
        {
            var match = Regex.Match(token, @"^[a-zA-Z][a-zA-Z0-9]*$");
            return match.Success;
        }

        private bool isReserved(string token)
        {
            return reservedTokens.Any(reservedToken => reservedToken.Equals(token));
        }

        private List<String> DeepTokenize(string line)
        {
            var tokenizedLine = new List<string>();
            tokenizedLine = line
                .Split(basicSeparators.ToArray(), StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            tokenizedLine = tokenizedLine
                .Select(part => RegularExpresionSplit(part, @"([;\(\)\[\],])"))
                .SelectMany(element => element)
                .ToList();

            tokenizedLine = tokenizedLine
                .Select(part => RegularExpresionSplit(part, @"(>=|>|<|!=|=|\+|\-|\*|\%|and|or)"))
                .SelectMany(element => element)
                .ToList();
            return tokenizedLine;

            /*return line
                //Split after separators
                .Split(separators.ToArray(), StringSplitOptions.RemoveEmptyEntries)
                //Split after composed operators
                .Select(token => token.Split(composedOperators.ToArray(), StringSplitOptions.RemoveEmptyEntries))
                .SelectMany(tokens => tokens) // flaten the resulting lists
                //Split after simple operators
                .Select(token => token.Split(operators.ToArray(), StringSplitOptions.RemoveEmptyEntries))
                .SelectMany(tokens => tokens) // flaten the resulting lists
                .ToList();*/

        }

        private List<String> RegularExpresionSplit(string input, string pattern)
        {
            var substrings = Regex.Split(input, pattern);
            return substrings
                .Where(part => part != "")
                .ToList();
        }
    }
}
